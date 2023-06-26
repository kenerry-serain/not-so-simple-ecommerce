namespace SimpleEcommerce.Order.Models;

public class OrderEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
