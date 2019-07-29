namespace ShoppingCartService.Commands {
    using System.Collections.Generic;
    using MediatR;
    
    public class DeleteShoppingCartCommand :IRequest<Unit>{
        public int UserId { get; set; }
    }
}