namespace SimpleEcommerceV2.Order.Domain.InOut.Requests
{
    public record AuthRequest(string Email, string Password, bool OnlyTokenBody=true) { }
}
