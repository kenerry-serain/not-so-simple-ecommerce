using NotSoSimpleEcommerce.Shared.Enums;

namespace NotSoSimpleEcommerce.Shared.Models;

public class OrderEntity
{
    public OrderEntity(int productId, int quantity, string boughtBy, OrderStatus statusId)
    {
        ProductId = productId;
        Quantity = quantity;
        BoughtBy = boughtBy;
        StatusId = statusId;
    }
    
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string BoughtBy{ get; set; }
    public OrderStatus StatusId{ get; set; }
    public ProductEntity Product { get; set; }
    public StatusEntity Status { get; set; }
}
