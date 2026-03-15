using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductCommands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            IApplicationDbContext context,
            ILogger<CreateProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating product. Name: {Name}, Price: {Price}",
                request.Name,
                request.Price);

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Product created successfully. ProductId: {ProductId}, Name: {Name}",
                product.Id,
                product.Name);

            return product.Id;
        }
    }
}