namespace InventoryService.Commands
{
    using MediatR;
    public class AddNewProductToInventoryCommandResult
    {
        public int InventoryStoreId { get; set; }

        public AddNewProductToInventoryCommandResult()
        {
            
        }
    }
}