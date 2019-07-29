namespace ShoppingCartService.Commands {
    public class ShoppingCartItemCommand {
        public int ShoppingCartItemId { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public long Quantity { get; set; }
    }
}