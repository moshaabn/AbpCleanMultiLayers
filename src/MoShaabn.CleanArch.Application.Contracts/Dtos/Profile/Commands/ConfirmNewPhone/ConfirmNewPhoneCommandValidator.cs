using FluentValidation;

namespace MoShaabn.CleanArch.Business.Client.Profile.Commands.ConfirmNewPhone;

public class ConfirmNewPhoneCommandValidator : AbstractValidator<ConfirmNewPhoneCommand>
{
    public ConfirmNewPhoneCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .Length(4).WithMessage("Code must be 4 digits long");
    }
}