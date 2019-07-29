namespace InventoryService.Domain {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract for accessing Inventory Store
    /// </summary>
    public interface IInventoryStoreRepository {

        Task<InventoryStore> FindById (int inventoryStoreId);
        Task<InventoryStore> FindByProductId (int productId);
        Task<IEnumerable<InventoryStore>> GetAll ();
        Task<int> Add (InventoryStore inventoryItem);
        Task Update (InventoryStore inventoryItem);
        Task Delete (int productId);
    }
}