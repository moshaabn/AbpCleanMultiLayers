using MoShaabn.CleanArch.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MoShaabn.CleanArch.Permissions;

public class CleanArchPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CleanArchPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(CleanArchPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CleanArchResource>(name);
    }
}
