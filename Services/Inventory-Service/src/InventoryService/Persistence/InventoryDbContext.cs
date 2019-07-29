namespace InventoryService.Persistence {
    using InventoryService.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// InventoryStore DbContext Class
    /// </summary>
    public class InventoryDbContext : DbContext {

       /// <summary>
       /// InventoryStore Table
       /// </summary>
       /// <value></value>
        public DbSet<InventoryStore> InventoryStore { get; set; }

        /// <summary>
        ///  Constructor to set and pass the DbContextOptions
        /// </summary>
        /// <param name="options"></param>
        public InventoryDbContext (DbContextOptions options) : base (options) {

        }

        /// <summary>
        /// Seed the database with initial values
        /// </summary>
        /// <param name="modelBuilder">To Define entities and their relationships</param>
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<InventoryStore> ().HasData (
                new InventoryStore { InventoryStoreId = 1, ProductName = "Chai", UnitPrice = 18m, UnitsInStock = 39, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 2, ProductName = "Chang", UnitPrice = 19m, UnitsInStock = 17, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 3, ProductName = "Aniseed Syrup", UnitPrice = 10m, UnitsInStock = 13, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 4, ProductName = "Chef Anton's Cajun Seasoning", UnitPrice = 22m, UnitsInStock = 53, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 5, ProductName = "Chef Anton's Gumbo Mix", UnitPrice = 21.35m, UnitsInStock = 0, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 6, ProductName = "Grandma's Boysenberry Spread", UnitPrice = 25m, UnitsInStock = 120, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 7, ProductName = "Uncle Bob's Organic Dried Pears", UnitPrice = 30m, UnitsInStock = 15, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 8, ProductName = " Northwoods Cranberry Sauce", UnitPrice = 40m, UnitsInStock = 6, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 9, ProductName = "Mishi Kobe Niku", UnitPrice = 97m, UnitsInStock = 29, IsProductActive = true },
                new InventoryStore { InventoryStoreId = 10, ProductName = " Ikura", UnitPrice = 31m, UnitsInStock = 31, IsProductActive = true }
            );
        }
    }
}