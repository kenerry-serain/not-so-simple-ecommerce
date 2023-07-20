using NotSoSimpleEcommerce.Shared.Enums;

namespace NotSoSimpleEcommerce.Shared.Models;

public class StatusEntity
{
    public StatusEntity(OrderStatus id, string description)
    {
        Id = id;
        Description = description;
    }

    public OrderStatus Id{ get; set; }
    public string Description { get; set; }
}
