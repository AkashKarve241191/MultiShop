namespace ShoppingCartService.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using ShoppingCartService.Domain;

    public class ShoppingCartDbContext : DbContext
    {
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }

        public ShoppingCartDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>().HasMany(cart=>cart.ShoppingCartItems);
        }
    }
}