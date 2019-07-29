namespace InventoryService.Events.Contracts
{
     using  InventoryService.Events.Abstract;

    /// <summary>
    /// Contract for ProductDeleted Event 
    /// </summary>
    public interface IProductDeletedEvent :IIntegrationEvent {
        int ProductId { get; }
    }
}