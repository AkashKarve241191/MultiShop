namespace ProductService.Queries
{
    using MediatR;
    using ProductService.Domain;

    /// <summary>
    /// FindProductByIdQuery query object
    /// </summary>
    /// <typeparam name="Product"></typeparam>
    public class FindProductByIdQuery : IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}