namespace token_authentication.Models
{
    public class User
    {
        public string? UserName { get; set; } 
        public string? Password { get; set; } 
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
