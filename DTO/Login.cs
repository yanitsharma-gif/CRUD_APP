namespace Practice.DTO
{
    public class Login
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
       
    }
}
