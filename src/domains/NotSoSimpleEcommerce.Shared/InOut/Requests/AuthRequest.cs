namespace NotSoSimpleEcommerce.Shared.InOut.Requests;

public record AuthRequest(string Email, string Password, bool OnlyTokenBody=false);

