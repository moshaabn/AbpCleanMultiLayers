using System;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MoShaabn.CleanArch.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum = Enum.GetNames(context.Type)
                    .Select(name => new OpenApiString(name))
                    .ToList<IOpenApiAny>();

                schema.Type = "string"; // Ensure Swagger treats it as a string
                schema.Format = null;
            }
        }
    }
}
