using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using ShoppingCartService.Domain;

namespace ShoppingCartService.Persistence {

    /// <summary>
    /// Shopping Cart Repository
    /// </summary>
    public class ShoppingCartRepository : IShoppingCartRepository{
        private readonly ShoppingCartDbContext _context;
        private readonly ILogger<ShoppingCartRepository> _logger;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public ShoppingCartRepository (ShoppingCartDbContext context, ILogger<ShoppingCartRepository> logger) {
            _context = context ??
                throw new ArgumentNullException (nameof (context));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        /// Find an ShoppingCartItem by ShoppingCartItemId
        /// </summary>
        /// <param name="shoppingCartItemId"></param>
        /// <returns></returns>
        public async Task<ShoppingCartItem> FindByItemId (int shoppingCartItemId) {

            ShoppingCartItem cartItem = await _context.ShoppingCartItems.AsNoTracking ().FirstOrDefaultAsync (item => item.ShoppingCartItemId == shoppingCartItemId);

            //Log Information 
            _logger.LogInformation ($"Executed query to find shoppingCartItem by shoppingCartItemId :{shoppingCartItemId}");

            return cartItem;
        }

        // <summary>
        /// Find all ShoppingCartItems in a ShoppingCart by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> FindByUserId (int userId) {

            ShoppingCart cart = await _context.ShoppingCart.AsNoTracking ().FirstOrDefaultAsync (c => c.UserId == userId);

            //Log Information
            _logger.LogInformation ($"Executed query to find shoppingCart for UserId :{userId}");

            if (cart == null) {
                //Log Warning and return shoppingcart
                _logger.LogWarning ($"Not Found : Shoppingcart for UserId : {userId}");
                return cart;
            }

            var items = await _context.ShoppingCartItems.AsNoTracking ().Where (x => x.ShoppingCartId == cart.ShoppingCartId).ToListAsync ();

            //Log Information
            _logger.LogInformation ($"Executed query to find ShoppingCartItems for ShoppingCartId :{cart.ShoppingCartId}");

            if (items == null) {
                //Log Warning and return shoppingcart
                _logger.LogWarning ($"Not Found : ShoppingCart Items in ShoppingCart for UserId : {userId}");
                return cart;
            }

            //Add items to ShoppingCart
            cart.ShoppingCartItems = items.ToList ();

            //Log Information 
            _logger.LogInformation ($"Added {items.Count()} items to ShoppingCart for UserId - {userId}");

            //Return ShoppingCart
            return cart;
        }

        /// <summary>
        /// Find all ShoppingCartItems in a ShoppingCart by ShoppingCartId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> FindById (int shoppingCartId) {

            ShoppingCart cart = await _context.ShoppingCart.AsNoTracking ().FirstOrDefaultAsync (u => u.ShoppingCartId == shoppingCartId);

            //Log Information
            _logger.LogInformation ($"Executed query to find shoppingCart with  ShoppingCartId :{shoppingCartId}");

            if (cart == null) {
                //Log Warning and return cart
                _logger.LogWarning ($"Not Found : Shoppingcart with  ShoppingCartId - {shoppingCartId}");
                return cart;
            }

            var items = await _context.ShoppingCartItems.AsNoTracking ().Where (x => x.ShoppingCartId == cart.ShoppingCartId).ToListAsync ();

            //Log Information
            _logger.LogInformation ($"Executed query to find ShoppingCartItems for ShoppingCartId :{cart.ShoppingCartId}");

            if (items == null) {
                //Log Warning and return cart
                _logger.LogWarning ($"Not Found : ShoppingCart Items in ShoppingCart with ShoppingCartId : {cart.ShoppingCartId}");
                return cart;
            }

            //Add items to ShoppingCart
            cart.ShoppingCartItems = items.ToList ();
            
            //Log Information
            _logger.LogInformation ($"Added {items.Count()} items to ShoppingCart with ShoppingCartId - {cart.ShoppingCartId}");

            //Return ShoppingCart
            return cart;
        }

        /// <summary>
        /// Add ShoppingCartItems to Shopping Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<int> Add (ShoppingCart cart) {
            //Add a new shoppingcart to context
            EntityEntry<ShoppingCart> entry = _context.Add (cart);

            //Save changes to store
            await _context.SaveChangesAsync ();

            //Log Information
            _logger.LogInformation ($"Executed command to add new shoppingcart with by id : {entry.Entity.ShoppingCartId} to store.");

            //Return ShoppingCartId
            return entry.Entity.ShoppingCartId;
        }

        /// <summary>
        /// Update ShoppingCartItems in a Shopping Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task Update (ShoppingCart cart) {
            ShoppingCart cartInDb = await FindById (cart.ShoppingCartId);

            if (cartInDb == null) {
                //Log Error and throw an ArgumentNullException
                _logger.LogError ($"Not Found : Shoppingcart with  ShoppingCartId - {cartInDb.ShoppingCartId}");
                throw new ArgumentNullException ($"Not Found : Shoppingcart with  ShoppingCartId - {cartInDb.ShoppingCartId}");
            }

            //Update shoppingcart in context
            _context.Set<ShoppingCart> ().Update (cart);

            //Detach Entity
            _context.Entry(cart).State = EntityState.Detached;

            //Save changes to Store
            await _context.SaveChangesAsync ();

            //Log Information
            _logger.LogInformation ($"Executed command to update Shoppingcart with  ShoppingCartId : {cartInDb.ShoppingCartId} to store:");
        }

        /// <summary>
        /// Delete all ShoppingCartItems in a Shopping Cart
        /// </summary>
        /// <param name="cart"></param>
        public async Task Remove (int userId) {

            ShoppingCart cartInDb = await FindByUserId(userId);

              if (cartInDb == null) {
               //Log Error and throw an ArgumentNullException
                _logger.LogWarning ($"Not Found : Shoppingcart for UserId : {userId}");
                throw new ArgumentNullException ($"Not Found : Shoppingcart for UserId : {userId}");
              }

            //Remove shoppingcart in context
            _context.Set<ShoppingCart> ().Remove (cartInDb);

            //Detach Entity
            _context.Entry(cartInDb).State = EntityState.Detached;

            //Save changes to Store
            await _context.SaveChangesAsync ();

            //Log Information
            _logger.LogInformation ($"Executed command to remove Shoppingcart with  ShoppingCartId : {cartInDb.ShoppingCartId} to store:");

        }
    }
}