using Microsoft.Extensions.Localization;
using MoShaabn.CleanArch.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MoShaabn.CleanArch;

[Dependency(ReplaceServices = true)]
public class CleanArchBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<CleanArchResource> _localizer;

    public CleanArchBrandingProvider(IStringLocalizer<CleanArchResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
