namespace ProductService.Commands
{
    using MediatR;

    /// <summary>
    ///  Use the class to create a new DeleteProduct Command 
    /// </summary>
    /// <typeparam name="Unit">returns void</typeparam>
    public class DeleteProductCommand :  IRequest<Unit>
    {
        public int ProductId { get; set; }
    }
}