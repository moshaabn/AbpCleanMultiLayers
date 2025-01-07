namespace MoShaabn.CleanArch.Dtos.Auth.Commands.ValidateRegister;

public class ValidateRegisterOtpCommand
{
    public string PhoneNumber { get; set; }

    public string Code { get; set; }

}
