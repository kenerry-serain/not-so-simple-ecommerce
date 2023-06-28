namespace SimpleEcommerceV2.Main.Domain.InOut.Requests
{
    public sealed record StockRequest(int ProductId, int Quantity)
    { }
}
