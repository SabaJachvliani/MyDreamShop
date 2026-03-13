using FluentValidation;

namespace Application.Commands.ReviewCommands.Create
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);

            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.Comment)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
