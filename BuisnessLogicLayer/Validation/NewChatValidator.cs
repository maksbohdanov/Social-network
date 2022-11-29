using BuisnessLogicLayer.Models;
using FluentValidation;

namespace BuisnessLogicLayer.Validation
{
    public class NewChatValidator: AbstractValidator<NewChatModel>
    {
        public NewChatValidator()
        {
            RuleFor(x => x.FirstUserId)
                .NotEmpty().WithMessage("FirstUserId cannot be empty.");

            RuleFor(x => x.SecondUserId)
                .NotEmpty().WithMessage("SecondUserId cannot be empty.");
        }
    }
}
