using BuisnessLogicLayer.Models.DTOs;
using FluentValidation;

namespace BuisnessLogicLayer.Validation
{    
    public class UserEditValidator : AbstractValidator<UserDto>
    {
        public UserEditValidator()
        {
            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("BirthDate cannot be empty.")
                .LessThan(p => DateTime.Now).WithMessage("Invalid BirthDate cannot be greater then current.");
        }
    }
}
