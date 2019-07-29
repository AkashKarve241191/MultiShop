namespace ProductService.Commands
{
    using MediatR;

    /// <summary>
    ///  Use the class to create a new AddNewProduct command 
    /// </summary>
    /// <typeparam name="AddNewProductResult">Create the result on execution of AddNewProduct Command</typeparam>
    public class AddNewProductCommand :  IRequest<AddNewProductResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
    }
}