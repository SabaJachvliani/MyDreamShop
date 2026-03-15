using Application.Common.Exeptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductCommands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            IApplicationDbContext context,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product. ProductId: {ProductId}", request.Id);

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Update failed. Product not found. ProductId: {ProductId}", request.Id);
                throw new NotFoundException("Product doesn't exist.");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product updated successfully. ProductId: {ProductId}", product.Id);

            return true;
        }
    }
}