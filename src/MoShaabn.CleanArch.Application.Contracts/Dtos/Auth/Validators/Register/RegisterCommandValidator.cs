using MoShaabn.CleanArch.Dtos.Auth.Commands.Register;
using FluentValidation;

namespace MoShaabn.CleanArch.Dtos.Auth.Validators.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            // Rule for Name: Must not be empty and should be between 2 and 50 characters.
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name field is mandatory.")
                .MinimumLength(2).WithMessage("The Name must contain at least 2 characters.")
                .MaximumLength(50).WithMessage("The Name must not exceed 50 characters.");

            // Rule for PhoneNumber: Must start with '+' followed by digits, and validation is optional.
            RuleFor(x => x.PhoneNumber)
           .NotEmpty().WithMessage("Phone number is required.")  // Make the phone number required
           //regex for phone number for 96698898898
            .Matches(@"^\d+$").WithMessage("Phone number is not valid.")
           .When(x => !string.IsNullOrEmpty(x.PhoneNumber)); // This line is now redundant as NotEmpty ensures the field is not null or empty.

            // Rule for UserGender: Must be a valid enum value.
            RuleFor(x => x.UserGender)
          .IsInEnum().WithMessage("User gender must be a valid value.");

            // Rule for ProfileImage: Must be a valid image file and not exceed 8MB.
            // RuleFor(x => x.ProfileImage)
            //     .NotNull().WithMessage("Profile image is required.")
            //     .Must(x => x.ContentType == "image/jpeg" || x.ContentType == "image/jpg" || x.ContentType == "image/png")
            //     .WithMessage("Profile image must be in jpg, jpeg, or png format.")
            //     .Must(x => x.Length <= 8 * 1024 * 1024)
            //     .WithMessage("Profile image size must not exceed 8MB.");

           
        }
    }
}
