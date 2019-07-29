namespace ProductService.Persistence {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using ProductService.Domain;

    /// <summary>
    ///  Product Repository
    /// </summary>
    public class ProductRepository : IProductRepository {
        private readonly ProductDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        /// <summary>
        /// Constructor to inject database context
        /// </summary>
        /// <param name="context">ProductDbContext Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ProductRepository (ProductDbContext context, ILogger<ProductRepository> logger) {
            _context = context ??
                throw new ArgumentNullException (nameof (context));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        /// Returns a Product based on productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product> FindById (int productId) {
            //Find Product by productId
            Product product = await _context.Product.AsNoTracking ().FirstOrDefaultAsync (p => p.ProductId == productId);

            //Log Information
            _logger.LogInformation ($"Executed query to find product by productId :{productId}");

            //Return Product
            return product;
        }

        /// <summary>
        /// Returns all the Products in the DB Store/Repository
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAll () {
            //Get all products in store
            IEnumerable<Product> products = await _context.Set<Product> ().AsNoTracking ().ToListAsync ();

            //Log Information
            _logger.LogInformation ($"Executed query to get all products in store");

            //Return IEnumerable<Product> products
            return products;
        }

        /// <summary>
        /// Add a Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<int> Add (Product product) {
            //Add a new product to context
            EntityEntry<Product> entry = _context.Add (product);

            //Save changes to store
            await _context.SaveChangesAsync ();

            //Log Information
            _logger.LogInformation ($"Executed command to add a new product with by id : {entry.Entity.ProductId} to store:");

            //Return ProductId
            return entry.Entity.ProductId;
        }

        /// <summary>
        /// Update an Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task Update (Product product) {
            // Find the product in store by productId
            Product productInDb = await FindById (product.ProductId);

            if (productInDb == null) {
                //Log Error and throw an ArgumentNullException
                _logger.LogError ($"Not Found : Product with ProductId:{product.ProductId}");
                throw new ArgumentNullException ($"Not Found : Product with ProductId:{product.ProductId}");
            }

            //Update product in context
            _context.Set<Product> ().Update (product);

            //Log Information
            _logger.LogInformation ($"Executed command to update product for ProductId : {product.ProductId} to store:");

            //Save changes to Store
            await _context.SaveChangesAsync ();
        }

        /// <summary>
        /// Delete a Product from the DB Store/Repository
        /// </summary>
        /// <param name="productId"></param>
        public async Task Delete (int productId) {
            // Find the product in store by productId
            Product product = await FindById (productId);

            if (product == null) {
                //Log Error and throw an ArgumentNullException
                _logger.LogError ($"Not Found : Product with ProductId:{productId}");
                throw new ArgumentNullException ($"Not Found : Product with ProductId:{productId}");
            }

            //Remove Product from context
            _context.Set<Product> ().Remove (product);

            //Log Information
            _logger.LogInformation ($"Executed command to delete product with productId : {productId} from store.");

            //Save changes to Store
            await _context.SaveChangesAsync ();
        }
    }
}