namespace ProductService.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using ProductService.Domain;

    /// <summary>
    ///  ProductDb context class
    /// </summary>
    public class ProductDbContext : DbContext
    {
        /// <summary>
        ///  Product Table
        /// </summary>
        /// <value></value>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        ///  Constructor to set and pass the DbContextOptions
        /// </summary>
        /// <param name="options"></param>
        public ProductDbContext(DbContextOptions options) : base(options)
        {

        }

        /// <summary>
        /// Seed the database with initial values
        /// </summary>
        /// <param name="modelBuilder">To Define entities and their relationships</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Chai", Description = "10 boxes x 20 bags", UnitPrice = 18m, UnitsInStock = 39, IsActive = true },
                new Product { ProductId = 2, Name = "Chang", Description = "24 - 12 oz bottles", UnitPrice = 19m, UnitsInStock = 17, IsActive = true },
                new Product { ProductId = 3, Name = "Aniseed Syrup", Description = "12 - 550 ml bottles", UnitPrice = 10m, UnitsInStock = 13, IsActive = true },
                new Product { ProductId = 4, Name = "Chef Anton's Cajun Seasoning", Description = "48 - 6 oz jars", UnitPrice = 22m, UnitsInStock = 53, IsActive = true },
                new Product { ProductId = 5, Name = "Chef Anton's Gumbo Mix", Description = "36 boxes", UnitPrice = 21.35m, UnitsInStock = 0, IsActive = true },
                new Product { ProductId = 6, Name = "Grandma's Boysenberry Spread", Description = "12 - 8 oz jars", UnitPrice = 25m, UnitsInStock = 120, IsActive = true },
                new Product { ProductId = 7, Name = "Uncle Bob's Organic Dried Pears", Description = "12 - 1 lb pkgs", UnitPrice = 30m, UnitsInStock = 15, IsActive = true },
                new Product { ProductId = 8, Name = " Northwoods Cranberry Sauce", Description = "12 - 12 oz jars", UnitPrice = 40m, UnitsInStock = 6, IsActive = true },
                new Product { ProductId = 9, Name = "Mishi Kobe Niku", Description = "18 - 500 g pkgs", UnitPrice = 97m, UnitsInStock = 29, IsActive = true },
                new Product { ProductId = 10, Name = " Ikura", Description = " 12 - 200 ml jars  ", UnitPrice = 31m, UnitsInStock = 31, IsActive = true }
            );
        }
    }
}