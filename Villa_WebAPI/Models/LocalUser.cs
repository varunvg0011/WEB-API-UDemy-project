namespace Villa_WebAPI.Models
{
    public class LocalUser
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
