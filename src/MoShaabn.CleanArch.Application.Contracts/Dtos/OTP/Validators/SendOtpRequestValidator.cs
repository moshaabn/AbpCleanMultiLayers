using MoShaabn.CleanArch.Business.OTP.Commands.SendOtp;
using FluentValidation;

namespace MoShaabn.CleanArch.Business.OTP.Validators;

public class SendOtpRequestValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
             .NotEmpty().WithMessage("Phone number is required.");
    }
}