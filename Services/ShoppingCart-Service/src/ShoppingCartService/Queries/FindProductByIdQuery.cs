namespace ShoppingCartService.Queries
{
    using MediatR;
    using ShoppingCartService.Domain;

    public class FindProductByIdQuery : IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}