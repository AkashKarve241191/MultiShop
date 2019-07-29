namespace InventoryService.Events {
    using System;
    using InventoryService.Events.Contracts;
    using MediatR;

    public class ProductAddedEvent : IProductAddedEvent, INotification {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }

        public ProductAddedEvent () {

        }

        /// <summary>
        /// Constructor for initialization
        /// </summary>
        /// <param name="productId"> Product Id</param>
        /// <param name="name">Name of the Product</param>
        /// <param name="description">Description of the Product</param>
        /// <param name="unitPrice">Unit Price</param>
        /// <param name="unitsInStock">Units in Stock</param>
        /// <param name="isActive">Is Product Active or is discontinued</param>
        public ProductAddedEvent (int productId, string name, string description, decimal unitPrice, long unitsInStock, bool isActive, Guid id, DateTime creationDate) {
            ProductId = productId;
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            UnitsInStock = unitsInStock;
            IsActive = isActive;
            Id = id;
            CreationDate = creationDate;
        }
    }
}