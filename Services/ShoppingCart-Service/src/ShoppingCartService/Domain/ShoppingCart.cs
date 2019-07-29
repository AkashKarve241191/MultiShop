namespace ShoppingCartService.Domain {
    using System.Collections.Generic;
    public class ShoppingCart {
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}