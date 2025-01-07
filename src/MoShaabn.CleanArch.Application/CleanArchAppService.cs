using System;
using System.Collections.Generic;
using System.Text;
using MoShaabn.CleanArch.Localization;
using Volo.Abp.Application.Services;

namespace MoShaabn.CleanArch;

/* Inherit your application services from this class.
 */
public abstract class CleanArchAppService : ApplicationService
{
    protected CleanArchAppService()
    {
        LocalizationResource = typeof(CleanArchResource);
    }
}
