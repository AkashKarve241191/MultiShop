namespace ShoppingCartService.Services.Abstract
{
    using System.Threading.Tasks;
    using ShoppingCartService.Domain;

    public interface IProductService
    {
        Task<Product> FindProductById(int productId);
    }
}