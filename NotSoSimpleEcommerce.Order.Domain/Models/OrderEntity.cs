using NotSoSimpleEcommerce.Shared.Enums;

namespace NotSoSimpleEcommerce.Order.Domain.Models;

public class OrderEntity
{
    public OrderEntity(int productId, int quantity, string boughtBy, OrderStatus status)
    {
        ProductId = productId;
        Quantity = quantity;
        BoughtBy = boughtBy;
        Status = status;
    }
    
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string BoughtBy{ get; set; }
    public OrderStatus Status{ get; set; }
}
