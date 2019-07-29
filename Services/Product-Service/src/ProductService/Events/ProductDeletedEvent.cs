namespace ProductService.Events {
    using System;
    using MediatR;
    using ProductService.Events.Contracts;

    /// <summary>
    ///  Product Deleted Event class
    /// </summary>
    public class ProductDeletedEvent : IProductDeletedEvent, INotification {
        public int ProductId { get; }
        public Guid Id => Guid.NewGuid();
        public DateTime CreationDate => DateTime.UtcNow;

        /// <summary>
        /// Constructor for initialization
        /// </summary>
        /// <param name="productId"></param>
        public ProductDeletedEvent (int productId) {
            ProductId = productId;
        }
    }
}