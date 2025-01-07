using Volo.Abp.Modularity;

namespace MoShaabn.CleanArch;

[DependsOn(
    typeof(CleanArchApplicationModule),
    typeof(CleanArchDomainTestModule)
)]
public class CleanArchApplicationTestModule : AbpModule
{

}
