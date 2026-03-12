using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.DBContext.MyDreamShopDb
{
    public class MyDreamShopDbContext : DbContext, IApplicationDbContext
    {
        public MyDreamShopDbContext(DbContextOptions<MyDreamShopDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDreamShopDbContext).Assembly);
        }
    }
}
