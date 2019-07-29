namespace ProductService.Events.Contracts {
    using System;

    /// <summary>
    /// Contract for ProductDeleted Event 
    /// </summary>
    public interface IProductDeletedEvent :IIntegrationEvent {
        int ProductId { get; }
    }
}