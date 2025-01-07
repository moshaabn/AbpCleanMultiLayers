using MoShaabn.CleanArch.Localization;
using Volo.Abp.Application.Services;

namespace MoShaabn.CleanArch;

/* Inherit your application services from this class.
 */
public abstract class BaseAppService : ApplicationService
{
    protected BaseAppService()
    {
        LocalizationResource = typeof(CleanArchResource);
    }
}
