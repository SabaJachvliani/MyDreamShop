using MediatR;

namespace Application.Commands.ReviewCommands.Create
{
    public class CreateReviewCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
