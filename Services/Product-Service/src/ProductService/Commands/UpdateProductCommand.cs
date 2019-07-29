namespace ProductService.Commands
{
    using MediatR;
    /// <summary>
    ///   Use the class to create a new UpdateProduct command 
    /// </summary>
    /// <typeparam name="Unit"></typeparam>
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public bool IsActive { get; set; }
    }
}