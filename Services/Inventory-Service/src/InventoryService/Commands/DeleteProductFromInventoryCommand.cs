namespace InventoryService.Commands
{
    using MediatR;
    public class DeleteProductFromInventoryCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        
    }
}