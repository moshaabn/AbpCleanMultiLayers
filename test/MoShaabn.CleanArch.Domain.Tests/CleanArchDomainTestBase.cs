using Volo.Abp.Modularity;

namespace MoShaabn.CleanArch;

/* Inherit from this class for your domain layer tests. */
public abstract class CleanArchDomainTestBase<TStartupModule> : CleanArchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
