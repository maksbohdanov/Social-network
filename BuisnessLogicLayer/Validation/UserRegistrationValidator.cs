using BuisnessLogicLayer.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BuisnessLogicLayer.Validation
{
    public class UserRegistrationValidator: AbstractValidator<UserRegistrationModel>
    {
        private readonly Regex regex = new Regex(@"^[a-zA-Z]+$");
        public UserRegistrationValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name cannot be empty.")
                .MaximumLength(30).WithMessage("Maximmum length for First Name is 30 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name cannot be empty.")
                .MaximumLength(30).WithMessage("Maximmum length for Last Name is 30 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City cannot be empty.")
                .MaximumLength(30).WithMessage("Maximmum length for City is 30 characters.")
                .Matches(regex).WithMessage("City can contain only latin letters");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("BirthDate cannot be empty.")
                .LessThan(p => DateTime.Now).WithMessage("Invalid BirthDate cannot be greater then current.");
        }
    }
}
