namespace NotSoSimpleEcommerce.IdentityServer.Domain.Models
{
    public class UserEntity
    {
        public UserEntity(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public int Id { get; set; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
