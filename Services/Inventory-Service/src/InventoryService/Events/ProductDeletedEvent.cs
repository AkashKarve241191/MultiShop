namespace InventoryService.Events {
    using System;
    using MediatR;
    using InventoryService.Events.Contracts;

    public class ProductDeletedEvent : IProductDeletedEvent ,INotification{
        public int ProductId { get; set; }

        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}