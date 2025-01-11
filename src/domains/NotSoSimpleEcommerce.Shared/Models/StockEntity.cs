namespace NotSoSimpleEcommerce.Shared.Models
{
    public class StockEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductEntity Product { get; set; }
    }
}
