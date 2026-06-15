namespace Practice.Models
{
    public class Login
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }

        public string Password { get; set; } = string.Empty;
       
    }
}
