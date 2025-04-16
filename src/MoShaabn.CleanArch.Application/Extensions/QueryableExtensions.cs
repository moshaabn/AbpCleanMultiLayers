using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MoShaabn.CleanArch.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResultDto<T>> WithPagingOptions<T>(this IQueryable<T> query,
            FilterPagedRequest pageRequest, CancellationToken cancellationToken = default)
        {
            if (pageRequest.Sorting != null)
            {
                var sortingArray = pageRequest.Sorting.Split(" ");
                var field = sortingArray[0];
                var direction = sortingArray.Count() > 1? sortingArray[1] : "ASC";
                var propInfo = GetPropertyInfo(typeof(T), field);
                var expr = GetOrderExpression(typeof(T), propInfo);

                MethodInfo method;

                if (direction == "DESC")
                {
                    method = typeof(Queryable).GetMethods()
                        .FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
                }
                else
                {
                    method = typeof(Queryable).GetMethods()
                        .FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
                }

                var genericMethod = method!.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                query = (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
            }

            if (pageRequest.Filter != null && !string.IsNullOrEmpty(GetTerm(pageRequest.Filter)))
            {
                if (!pageRequest.Filter.Contains(","))
                {
                    var term = GetTerm(pageRequest.Filter);
                    var operation = GetOperation(pageRequest.Filter);
                    var field = GetField(pageRequest.Filter);
                    if (GetOperation(operation, term) != "none")
                    {
                        if (string.IsNullOrEmpty(field))
                        {
                            var propertiesInfo = typeof(T).GetProperties().Where(c => c.PropertyType == typeof(string))
                                .ToList();
                            query = query!.Where(GetWhereExpression<T, bool>(term,
                                operation, propertiesInfo.Select(c => c.Name).ToArray()));
                        }
                        else
                        {
                            var propInfo = GetPropertyName(typeof(T), field);
                            query = query!.Where(GetWhereExpression<T, bool>(term,
                                operation, propInfo));
                        }
                    }
                }

                if (pageRequest.Filter.Contains(","))
                {
                    // Multiple filters case (OR logic)
                    var filters = pageRequest.Filter.Split(',').Select(c => c.Trim()).ToList();
                    var orExpressions = new List<Expression<Func<T, bool>>>();

                    foreach (var filter in filters)
                    {
                        var term = GetTerm(filter);
                        var operation = GetOperation(filter);
                        var field = GetField(filter);

                        if (GetOperation(operation, term) != "none")
                        {
                            var propInfo = GetPropertyName(typeof(T), field);
                            var expression = GetWhereExpression<T, bool>(term, operation, propInfo);
                            orExpressions.Add(expression);
                        }
                    }

                    // Combine all expressions using OR
                    if (orExpressions.Any())
                    {
                        var combinedExpression = orExpressions.Aggregate((expr1, expr2) => CombineExpressions(expr1, expr2, Expression.And));
                        query = query!.Where(combinedExpression);
                    }
                }
            }

            int count = await query!.CountAsync(cancellationToken);
            var result = await query.Skip(pageRequest.SkipCount).Take(pageRequest.MaxResultCount).ToListAsync(cancellationToken);
            return new PagedResultDto<T>(count, result);
        }

        public static async Task<PagedResultDto<T>> WithPagingOptions<T>(this IQueryable<T> query,
          PagedAndSortedResultRequestDto pageRequest, CancellationToken cancellationToken = default)
        {
            if (pageRequest.Sorting != null)
            {
                var field = pageRequest.Sorting.Split(" ")[0];
                var direction = pageRequest.Sorting.Split(" ")[1];
                var propInfo = GetPropertyInfo(typeof(T), field);
                var expr = GetOrderExpression(typeof(T), propInfo);

                MethodInfo method;

                if (direction == "DESC")
                {
                    method = typeof(Queryable).GetMethods()
                        .FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
                }
                else
                {
                    method = typeof(Queryable).GetMethods()
                        .FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
                }

                var genericMethod = method!.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                query = (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
            }

            int count = await query!.CountAsync(cancellationToken);
            var result = await query.Skip(pageRequest.SkipCount).Take(pageRequest.MaxResultCount).ToListAsync(cancellationToken);
            return new PagedResultDto<T>(count, result);
        }

        private static string GetPropertyName(Type objType, string name)
        {
            if (name.Contains("."))
                return name;
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (matchedProperty == null)
                throw new ArgumentException($"could not find the field {name}");

            return matchedProperty.Name;
        }


        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (matchedProperty == null)
                throw new ArgumentException($"could not find the field {name}");

            return matchedProperty;
        }


        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        private static Expression<Func<T, TResult>> GetWhereExpression<T, TResult>(string term, string operation, params string[] propertiesNamesList)
        {
            var stringExpressions = new StringBuilder();
            foreach (var propertyName in propertiesNamesList)
            {
                if (stringExpressions.Length > 0)
                {
                    stringExpressions.Append(" && ");
                }

                stringExpressions.Append($"{propertyName}{GetOperation(operation, term)}");
            }

            return GetSelectorExpression<T, TResult>(stringExpressions.ToString());
        }


        public static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2,
            Func<Expression, Expression, BinaryExpression> combiner)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(combiner(left, right), parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _oldParameter)
                    return _newParameter;
                return base.VisitParameter(node);
            }
        }
        private static Expression<Func<TEntity, TResult>> GetSelectorExpression<TEntity, TResult>(
            string selectorExpression)
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var result =
                (Expression<Func<TEntity, TResult>>)DynamicExpressionParser.ParseLambda(new[] { parameterExpression },
                    typeof(TResult), selectorExpression);

            return result;
        }

        public static List<string> ToListFromSplitterString(this string sentence, string splitter)
        {
            if (string.IsNullOrEmpty(sentence))
                return new List<string>();
            var result = sentence.Split(splitter).ToList();
            result.ForEach(x => x = x.Trim());
            return result;
        }

        public static string GetField(string filter)
        {
            string[] result = filter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return result[0].Trim();
        }

        public static string GetOperation(string filter)
        {
            // Split the filter by spaces and remove empty entries
            string[] terms = filter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Ensure the result has three elements
            string[] result = new string[3];

            for (int i = 0; i < 3; i++)
            {
                result[i] = i < terms.Length ? terms[i].Trim() : string.Empty;
            }

            return result[1].Trim();
        }

        public static string GetTerm(string filter)
        {
            return filter.Substring(filter.IndexOf(' ', filter.IndexOf(' ') + 1) + 1).Trim();

        }


        private static string GetOperation(string operation, string term)
        {
            if (operation == "contains")
            {
                return $".ToLower().Contains(\"{term.ToLower()}\")";
            }
            if (operation == "equals")
            {
                return $" eq \"{term}\"";
            }
            if (operation == "greaterThan")
            {
                return $" gt \"{term}\"";
            }
            if (operation == "lessThan")
            {
                return $" lt \"{term}\"";
            }

            return "none";


        }
    }
}
