using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces

{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Product> Products { get; }    
        DbSet<Review> Reviews { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
