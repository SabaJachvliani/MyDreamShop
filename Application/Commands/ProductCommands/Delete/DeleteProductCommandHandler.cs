using Application.Common.Exeptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductCommands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(
            IApplicationDbContext context,
            ILogger<DeleteProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting product. ProductId: {ProductId}", request.Id);

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Delete failed. Product not found. ProductId: {ProductId}", request.Id);
                throw new NotFoundException("Product doesn't exist.");
            }

            product.DeletedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product deleted successfully. ProductId: {ProductId}", request.Id);

            return true;
        }
    }
}