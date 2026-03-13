using MediatR;

namespace Application.Commands.ProductCommands.Delete
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
