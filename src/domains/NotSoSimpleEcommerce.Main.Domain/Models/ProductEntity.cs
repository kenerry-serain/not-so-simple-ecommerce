namespace NotSoSimpleEcommerce.Main.Domain.Models
{
    public sealed class ProductEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
