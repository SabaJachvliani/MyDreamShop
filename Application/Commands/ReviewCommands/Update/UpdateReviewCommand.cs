using MediatR;

namespace Application.Commands.ReviewCommands.Update
{
    public class UpdateReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
