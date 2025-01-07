using MoShaabn.CleanArch.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MoShaabn.CleanArch.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CleanArchEntityFrameworkCoreModule),
    typeof(CleanArchApplicationContractsModule)
    )]
public class CleanArchDbMigratorModule : AbpModule
{
}
