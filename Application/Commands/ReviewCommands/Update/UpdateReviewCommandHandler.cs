using Application.Common.Exeptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ReviewCommands.Update
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UpdateReviewCommandHandler> _logger;

        public UpdateReviewCommandHandler(
            IApplicationDbContext context,
            ILogger<UpdateReviewCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Updating review. ReviewId: {ReviewId}, ProductId: {ProductId}, UserId: {UserId}",
                request.Id,
                request.ProductId,
                request.UserId);

            var review = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            if (review == null)
            {
                _logger.LogWarning(
                    "Update review failed. Review not found. ReviewId: {ReviewId}",
                    request.Id);

                throw new NotFoundException("Review doesn't exist.");
            }

            var productExists = await _context.Products
                .AnyAsync(x => x.Id == request.ProductId && x.DeletedAt == null, cancellationToken);

            if (!productExists)
            {
                _logger.LogWarning(
                    "Update review failed. Product not found. ProductId: {ProductId}",
                    request.ProductId);

                throw new NotFoundException("Product not found.");
            }

            review.ProductId = request.ProductId;
            review.UserId = request.UserId;
            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Review updated successfully. ReviewId: {ReviewId}",
                review.Id);

            return true;
        }
    }
}