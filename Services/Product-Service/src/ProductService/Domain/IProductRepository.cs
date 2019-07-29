namespace ProductService.Domain
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    /// <summary>
    /// Contract for accessing Product from store
    /// </summary>
    public interface IProductRepository
    {
        Task<Product> FindById(int productId);
        Task<IEnumerable<Product>> GetAll();
        Task<int> Add(Product product);
        Task Update(Product product);
        Task Delete(int productId);

    }
}