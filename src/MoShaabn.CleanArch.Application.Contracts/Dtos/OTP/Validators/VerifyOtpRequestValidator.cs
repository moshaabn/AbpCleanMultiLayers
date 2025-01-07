using MoShaabn.CleanArch.Business.OTP.Commands.VerifyOtp;
using FluentValidation;

namespace MoShaabn.CleanArch.Business.OTP.Validators;

public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpRequestValidator()
    {


        RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");

        RuleFor(x => x.Otp).NotEmpty().Matches(@"^\d{4}$");


    }
}