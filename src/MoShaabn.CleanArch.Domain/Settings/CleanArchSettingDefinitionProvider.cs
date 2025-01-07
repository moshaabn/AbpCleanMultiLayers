using Volo.Abp.Settings;

namespace MoShaabn.CleanArch.Settings;

public class CleanArchSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(CleanArchSettings.MySetting1));
    }
}
