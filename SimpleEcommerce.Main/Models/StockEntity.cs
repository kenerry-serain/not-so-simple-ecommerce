namespace SimpleEcommerce.Main.Models
{
    public sealed class StockEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal StockEntity ReduceQuantity(int quantity)
        {
            Quantity = quantity - Quantity;
            return this;    
        }
    }
}
