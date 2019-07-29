namespace ShoppingCartService.Queries
{
    using MediatR;
    using ShoppingCartService.Domain;

    public class FindShoppingCartByUserIdQuery : IRequest<ShoppingCart>
    {
        public int UserId { get; set; }
    }
}