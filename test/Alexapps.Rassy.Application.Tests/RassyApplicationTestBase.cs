using Volo.Abp.Modularity;

namespace MoShaabn.CleanArch;

public abstract class CleanArchApplicationTestBase<TStartupModule> : CleanArchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
