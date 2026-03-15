using Application.Common.Exeptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ReviewCommands.Create
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(
            IApplicationDbContext context,
            ILogger<CreateReviewCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating review. ProductId: {ProductId}, UserId: {UserId}, Rating: {Rating}",
                request.ProductId,
                request.UserId,
                request.Rating);

            var productExists = await _context.Products
                .AnyAsync(x => x.Id == request.ProductId && x.DeletedAt == null, cancellationToken);

            if (!productExists)
            {
                _logger.LogWarning(
                    "Create review failed. Product not found. ProductId: {ProductId}",
                    request.ProductId);

                throw new NotFoundException("Product not found.");
            }

            var review = new Review
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Review created successfully. ReviewId: {ReviewId}, ProductId: {ProductId}, UserId: {UserId}",
                review.Id,
                review.ProductId,
                review.UserId);

            return review.Id;
        }
    }
}