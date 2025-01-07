using MoShaabn.CleanArch.Business.Client.Profile.Commands.EditInfo;
using FluentValidation;

namespace MoShaabn.CleanArch.Business.Client.Profile.Results;

public class EditClientInfoCommandValidator : AbstractValidator<EditClientInfoCommand>
{
    public EditClientInfoCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(\+965|965)?[569]\d{7}$")
            .WithMessage("Invalid phone number format."); 
        
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email address format.")
            .When(x => !string.IsNullOrEmpty(x.Email));
        
        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City is required.");
        
        RuleFor(x => x.NeighborhoodId)
            .NotEmpty().WithMessage("Neighborhood is required.");
    }
}