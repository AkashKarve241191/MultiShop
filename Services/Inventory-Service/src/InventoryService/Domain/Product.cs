namespace InventoryService.Domain
{
    /// <summary>
    ///  Product Class
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public bool IsActive { get; set; }
    }
}