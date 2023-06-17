namespace Villa_WebAPI.Models.DTO
{
    public class LoginResponseDTO
    {
        //public LocalUser User { get; set; }
        //we dont require the above after adding Identity so added below
        public UserDTO User { get; set; }
        public string Role { get; set; }

        public string Token { get; set; }
    }
}
