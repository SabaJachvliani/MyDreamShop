using Application.Common.Exeptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ReviewCommands.Delete
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;

        public DeleteReviewCommandHandler(
            IApplicationDbContext context,
            ILogger<DeleteReviewCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting review. ReviewId: {ReviewId}", request.Id);

            var review = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            if (review == null)
            {
                _logger.LogWarning("Delete review failed. Review not found. ReviewId: {ReviewId}", request.Id);
                throw new NotFoundException("Review doesn't exist.");
            }

            review.DeletedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Review deleted successfully. ReviewId: {ReviewId}", request.Id);

            return true;
        }
    }
}