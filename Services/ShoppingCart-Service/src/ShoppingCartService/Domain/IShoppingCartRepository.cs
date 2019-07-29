namespace ShoppingCartService.Domain
{
    using System.Threading.Tasks;

    public interface IShoppingCartRepository
    {
         Task<ShoppingCartItem> FindByItemId (int shoppingCartItemId);
         Task<ShoppingCart> FindByUserId (int userId);
         Task<ShoppingCart> FindById (int shoppingCartId);
         Task<int> Add (ShoppingCart cart);
         Task Update (ShoppingCart cart);
         Task Remove (int shoppingCartId);
         
    }
}