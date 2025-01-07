using MoShaabn.CleanArch.Dtos.Auth.Commands.ResendPhoneConfirmation;
using FluentValidation;

namespace MoShaabn.CleanArch.Dtos.Auth.Validators.ResendPhoneConfirmation;

public class ResendPhoneConfirmationCommandValidator : AbstractValidator<ResendPhoneConfirmationCommand>
{
    public ResendPhoneConfirmationCommandValidator()
    {
        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(20);
    }
}
