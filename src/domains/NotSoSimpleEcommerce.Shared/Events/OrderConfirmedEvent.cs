namespace NotSoSimpleEcommerce.Shared.Events;

public record OrderConfirmedEvent(int Id)
{
    public OrderConfirmedEvent(): this(int.MinValue){}
}
