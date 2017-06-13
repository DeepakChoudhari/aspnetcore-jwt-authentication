namespace AspnetCore.Jwt.Authentication.Models
{
    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; internal set; }
    }
}