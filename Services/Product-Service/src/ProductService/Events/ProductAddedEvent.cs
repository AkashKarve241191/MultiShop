namespace ProductService.Events {
    using System;
    using MediatR;
    using Newtonsoft.Json;
    using ProductService.Events.Contracts;

    /// <summary>
    ///  Product Added Event class 
    /// </summary>

    public class ProductAddedEvent : IProductAddedEvent, INotification {

        public int ProductId { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal UnitPrice { get; }
        public long UnitsInStock { get; }
        public bool IsActive { get; }
        public Guid Id => Guid.NewGuid ();
        public DateTime CreationDate => DateTime.UtcNow;

        /// <summary>
        /// Constructor for initialization
        /// </summary>
        /// <param name="productId"> Product Id</param>
        /// <param name="name">Name of the Product</param>
        /// <param name="description">Description of the Product</param>
        /// <param name="unitPrice">Unit Price</param>
        /// <param name="unitsInStock">Units in Stock</param>
        /// <param name="isActive">Is Product Active or is discontinued</param>
        public ProductAddedEvent (int productId, string name, string description, decimal unitPrice, long unitsInStock, bool isActive) {
            ProductId = productId;
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            UnitsInStock = unitsInStock;
            IsActive = isActive;
        }
    }
}