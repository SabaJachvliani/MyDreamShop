using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.ProductQueries.GetByList
{
    public class ProductByListHandler : IRequestHandler<ProductByListQuery, List<ProductDetailsDto>>
    {
        private readonly IApplicationDbContext _db;
        private readonly ILogger<ProductByListHandler> _logger;

        public ProductByListHandler(
            IApplicationDbContext db,
            ILogger<ProductByListHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<ProductDetailsDto>> Handle(ProductByListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting product list");

            var products = await _db.Products
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
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
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Product list loaded successfully. Count: {Count}", products.Count);

            return products;
        }
    }
}