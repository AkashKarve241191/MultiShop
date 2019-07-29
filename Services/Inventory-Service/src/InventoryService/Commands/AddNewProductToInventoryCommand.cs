namespace InventoryService.Commands {
    using MediatR;

    public class AddNewProductToInventoryCommand : IRequest<AddNewProductToInventoryCommandResult> {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public bool IsActive { get; set; }

    }
}