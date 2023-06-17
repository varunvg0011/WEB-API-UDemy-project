using Microsoft.AspNetCore.Identity;

namespace Villa_WebAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name{ get; set; }
    }
}
