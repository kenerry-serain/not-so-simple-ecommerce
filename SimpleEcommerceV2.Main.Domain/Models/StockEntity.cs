namespace SimpleEcommerceV2.Main.Domain.Models
{
    public sealed class StockEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
