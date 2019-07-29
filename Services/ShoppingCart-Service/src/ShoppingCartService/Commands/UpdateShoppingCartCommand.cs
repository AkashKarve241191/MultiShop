namespace ShoppingCartService.Commands {
    using System.Collections.Generic;
    using MediatR;
    
    public class UpdateShoppingCartCommand :IRequest<Unit>{
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public List<ShoppingCartItemCommand> ShoppingCartItems { get; set; }
    }
}