namespace Villa_WebApp.Models.DTO
{
    public class UserDTO
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        //dont want this later in project when adding role from token itself
        //public string Role { get; set; }
    }
}
