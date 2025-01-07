namespace MoShaabn.CleanArch.Dtos.Auth.Commands.ForgetPassword;

public class ValidateForgetPasswordCommand
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}
