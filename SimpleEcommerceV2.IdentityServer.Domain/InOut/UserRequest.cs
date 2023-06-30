namespace SimpleEcommerceV2.IdentityServer.Domain.InOut
{
    public record AuthRequest(string Email, string Password, bool OnlyTokenBody=false) { }
}
