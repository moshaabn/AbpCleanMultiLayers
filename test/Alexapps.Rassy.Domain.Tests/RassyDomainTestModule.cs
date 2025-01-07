using Volo.Abp.Modularity;

namespace MoShaabn.CleanArch;

[DependsOn(
    typeof(CleanArchDomainModule),
    typeof(CleanArchTestBaseModule)
)]
public class CleanArchDomainTestModule : AbpModule
{

}
