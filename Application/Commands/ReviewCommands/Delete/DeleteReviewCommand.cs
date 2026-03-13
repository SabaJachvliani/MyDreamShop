using MediatR;

namespace Application.Commands.ReviewCommands.Delete
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
