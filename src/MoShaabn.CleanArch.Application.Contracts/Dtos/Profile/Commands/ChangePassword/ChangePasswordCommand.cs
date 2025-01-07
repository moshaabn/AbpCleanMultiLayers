namespace MoShaabn.CleanArch.Business.Client.Profile.Commands.ChangePassword;

public class ChangePasswordCommand
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}