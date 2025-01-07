using MoShaabn.CleanArch.Dtos.Auth.Commands.ForgetPassword;
using MoShaabn.CleanArch.Enums;
using FluentValidation;

namespace MoShaabn.CleanArch.Dtos.Auth.Validators.ForgetPassword;

public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordCommandValidator()
    {
        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(20);

        // Rule for Role: Must not be empty or null and must be a valid role (user or admin)
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .NotNull().WithMessage("Role cannot be null.")
            .Must(x => x == RoleEnum.USER || x == RoleEnum.ADMIN).WithMessage("Role is not valid.");
    }
}
