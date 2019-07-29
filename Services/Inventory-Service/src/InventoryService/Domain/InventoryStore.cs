namespace InventoryService.Domain {
    using System.Threading;
    using System;
    using InventoryService.Commands;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class InventoryStore {
        public int InventoryStoreId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public bool IsProductActive { get; set; }

    }
}