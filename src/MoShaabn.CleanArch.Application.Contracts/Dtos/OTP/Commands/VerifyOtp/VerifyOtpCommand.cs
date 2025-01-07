using MoShaabn.CleanArch.Enums;

namespace MoShaabn.CleanArch.Business.OTP.Commands.VerifyOtp;

public class VerifyOtpCommand
{
    public string PhoneNumber { get; init; }
    public string Otp { get; init; }
}