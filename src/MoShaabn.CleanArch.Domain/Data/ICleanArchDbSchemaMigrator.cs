using System.Threading.Tasks;

namespace MoShaabn.CleanArch.Data;

public interface ICleanArchDbSchemaMigrator
{
    Task MigrateAsync();
}
