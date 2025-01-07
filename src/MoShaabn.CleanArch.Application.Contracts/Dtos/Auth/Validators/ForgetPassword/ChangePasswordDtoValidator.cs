using MoShaabn.CleanArch.Dtos.Auth.Commands.ForgetPassword;
using FluentValidation;

namespace MoShaabn.CleanArch.Dtos.Auth.Validators.ForgetPassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.NewPassword)
            .NotEmpty()
            .NotNull()
            .Matches(@"^(?=.*[a-z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one lowercase letter, and one digit.");

    }
}
