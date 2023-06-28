namespace SimpleEcommerceV2.Order.Domain.InOut.Requests;

public record OrderRequest(int ProductId, int Quantity)
{ }
