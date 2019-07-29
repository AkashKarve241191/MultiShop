namespace InventoryService.Persistence {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using InventoryService.Domain;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///  InventoryStore Repository
    /// </summary>
    public class InventoryStoreRepository : IInventoryStoreRepository {

        private readonly InventoryDbContext _context;
        private readonly ILogger<InventoryStoreRepository> _logger;

        /// <summary>
        ///  Constructor for DI
        /// </summary>
        /// <param name="dbContext">InventoryDbContext Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public InventoryStoreRepository (InventoryDbContext context, ILogger<InventoryStoreRepository> logger) {
            _context = context ??
                throw new ArgumentNullException (nameof (context));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));

        }

        /// <summary>
        ///  Add a new Product to Inventory Store
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public async Task<int> Add (InventoryStore inventoryItem) {
            //Add a new product to context
            EntityEntry<InventoryStore> entry = _context.Add (inventoryItem);

            //Save changes to store
            await _context.SaveChangesAsync ();

            //Log Information
            _logger.LogInformation ($"Executed command to add a new product with ProductId:{inventoryItem.ProductId} and name : {inventoryItem.ProductName} to the Inventory Store with id : {entry.Entity.ProductId} ");

            //Return InventoryItemId
            return entry.Entity.InventoryStoreId;
        }

        /// <summary>
        ///  Delete a product from Inventory Store
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task Delete (int productId) {
            // Find the product in store by productId
            InventoryStore inventoryStoreItem = await FindByProductId (productId);

            if (inventoryStoreItem == null) {
                //Log Error and throw an ArgumentNullException
                _logger.LogError ($"Not Found : Product with ProductId:{productId} in Inventory Store");
                throw new ArgumentNullException ($"Not Found : Product with ProductId:{productId} in Inventory Store");
            }

            //Detach
            _context.Entry(inventoryStoreItem).State = EntityState.Detached;

            //Remove Product from context
            _context.Set<InventoryStore> ().Remove (inventoryStoreItem);

            //Log Information
            _logger.LogInformation ($"Executed command to delete product with productId : {productId} from store.");

            //Save changes to Store
            await _context.SaveChangesAsync ();
        }

        /// <summary>
        /// Find an item in Inventory Store by InventoryStore Id
        /// </summary>
        /// <param name="inventoryStoreId"></param>
        /// <returns></returns>
        public async Task<InventoryStore> FindById (int inventoryStoreId) {
            //Find Product by productId in Inventory Store
            InventoryStore inventoryStoreItem = await _context.InventoryStore.AsNoTracking ().FirstOrDefaultAsync (i => i.InventoryStoreId == inventoryStoreId);

            //Log Information
            _logger.LogInformation ($"Executed query to find Inventory Store Item by InventoryStoreId :{inventoryStoreId}.");

            //Return Product from Inventory Store
            return inventoryStoreItem;
        }

        /// <summary>
        /// Find an item in Inventory Store by Product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<InventoryStore> FindByProductId (int productId) {
            //Find Product by productId in Inventory Store
            InventoryStore inventoryStoreItem = await _context.InventoryStore.AsNoTracking ().FirstOrDefaultAsync (p => p.ProductId == productId);

            //Log Information
            _logger.LogInformation ($"Executed query to find Inventory Store Item by productId :{productId}.");

            //Return Product from Inventory Store
            return inventoryStoreItem;
        }

        /// <summary>
        ///  Get all items in Inventory Store
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<InventoryStore>> GetAll () {
            //Get all products in store
            IEnumerable<InventoryStore> inventoryStoreItems = await _context.Set<InventoryStore> ().AsNoTracking ().ToListAsync ();

            //Log Information
            _logger.LogInformation ($"Executed query to get all products in store");

            //Return IEnumerable<Product> products
            return inventoryStoreItems;
        }

        /// <summary>
        /// Update an item in Inventory Store
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public async Task Update (InventoryStore inventoryItem) {
            // Find the product in Inventory Store by productId
            InventoryStore inventoryItemInStore = await FindById (inventoryItem.ProductId);

            if (inventoryItemInStore == null) {
                //Log Error and throw an ArgumentNullException
                _logger.LogError ($"Not Found : Product with ProductId:{inventoryItem.ProductId} in Inventory Store");
                throw new ArgumentNullException ($"Not Found : Product with ProductId:{inventoryItem.ProductId} in Inventory Store");
            }

            //Update product in context
            _context.Set<InventoryStore> ().Update (inventoryItem);

            //Log Information
            _logger.LogInformation ($"Executed command to update product for ProductId : {inventoryItem.ProductId} to store:");

            //Save changes to Store
            await _context.SaveChangesAsync ();
        }
    }
}