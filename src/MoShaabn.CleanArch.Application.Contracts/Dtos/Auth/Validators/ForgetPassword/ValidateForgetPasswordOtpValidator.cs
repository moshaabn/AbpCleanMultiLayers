using MoShaabn.CleanArch.Dtos.Auth.Commands.ForgetPassword;
using FluentValidation;

namespace MoShaabn.CleanArch.Dtos.Auth.Validators.ForgetPassword;

public class ValidateForgetPasswordOtpValidator : AbstractValidator<ValidateForgetPasswordCommand>
{
    public ValidateForgetPasswordOtpValidator()
    {
        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(c => c.Code)
            .NotEmpty()
            .NotNull()
            .Length(4);
    }
}
