using BuisnessLogicLayer.Models;
using FluentValidation;

namespace BuisnessLogicLayer.Validation
{
    public class MessageValidator: AbstractValidator<MessageModel>
    {
        public MessageValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Message cannot be empty.")
                .MaximumLength(200).WithMessage("Maximmum length for message is 200 characters.");

            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("AuthorId cannot be empty.");

            RuleFor(x => x.ChatId)
                .NotEmpty().WithMessage("ChatId cannot be empty.");
        }
    }
}
