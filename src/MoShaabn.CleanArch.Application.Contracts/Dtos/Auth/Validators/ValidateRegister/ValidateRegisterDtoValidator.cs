using MoShaabn.CleanArch.Dtos.Auth.Commands.ValidateRegister;
using FluentValidation;

namespace MoShaabn.CleanArch.Dtos.Auth.Validators.ValidateRegister;

public class ValidateRegisterCommandValidator : AbstractValidator<ValidateRegisterOtpCommand>
{
    public ValidateRegisterCommandValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .NotNull()
            .Length(4);


        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(20);
    }
}
