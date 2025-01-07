using MoShaabn.CleanArch.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MoShaabn.CleanArch.Controllers
{
    [IgnoreAntiforgeryToken]
    public abstract class CleanArchController : AbpControllerBase
    {
        protected CleanArchController()
        {
            LocalizationResource = typeof(CleanArchResource);
        }
    }

}