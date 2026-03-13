using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ReviewCommands.Update
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReviewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            if (review == null)
                throw new Exception("Review doesn't exist");

            var productExists = await _context.Products
                .AnyAsync(x => x.Id == request.ProductId && x.DeletedAt == null, cancellationToken);

            if (!productExists)
                throw new Exception("Product not found");

            review.ProductId = request.ProductId;
            review.UserId = request.UserId;
            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
