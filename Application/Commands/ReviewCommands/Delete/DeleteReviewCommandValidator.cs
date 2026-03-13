using FluentValidation;

namespace Application.Commands.ReviewCommands.Delete
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
