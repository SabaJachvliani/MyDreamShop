using Application.Common.Exeptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.ProductQueries
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
    {
        private readonly IApplicationDbContext _db;
        private readonly ILogger<GetProductByIdHandler> _logger;

        public GetProductByIdHandler(
            IApplicationDbContext db,
            ILogger<GetProductByIdHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting product by id: {ProductId}", request.Id);

            var product = await _db.Products
                .AsNoTracking()
                .Where(x => x.Id == request.Id && x.DeletedAt == null)
                .Select(x => new ProductDetailsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description,

                    ReviewsCount = x.Reviews.Count(r => r.DeletedAt == null),

                    AverageRating = x.Reviews.Any(r => r.DeletedAt == null)
                        ? Math.Round(
                            x.Reviews
                                .Where(r => r.DeletedAt == null)
                                .Average(r => (double)r.Rating), 1)
                        : 0,

                    Reviews = x.Reviews
                        .Where(r => r.DeletedAt == null)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => new ProductReviewDto
                        {
                            Id = r.Id,
                            Rating = r.Rating,
                            Comment = r.Comment,
                            CreatedAt = r.CreatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product not found. ProductId: {ProductId}", request.Id);
                throw new NotFoundException("Product not found.");
            }

            _logger.LogInformation(
                "Product loaded successfully. ProductId: {ProductId}, ReviewsCount: {ReviewsCount}, AverageRating: {AverageRating}",
                product.Id,
                product.ReviewsCount,
                product.AverageRating);

            return product;
        }
    }
}