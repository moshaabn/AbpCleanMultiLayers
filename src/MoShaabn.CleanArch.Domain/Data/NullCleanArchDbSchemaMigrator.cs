using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MoShaabn.CleanArch.Data;

/* This is used if database provider does't define
 * ICleanArchDbSchemaMigrator implementation.
 */
public class NullCleanArchDbSchemaMigrator : ICleanArchDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
