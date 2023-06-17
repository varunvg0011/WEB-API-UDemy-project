using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Villa_WebAPI.Data;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;
using Villa_WebAPI.Repository.IRepository;

namespace Villa_WebAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private string secretKey;

        //userNamager is a built-in helper method used to accomplish Identity related tasks
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _db= db;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("AppSettings:Secret");
            _userManager = userManager;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            //var user = _db.LocalUsers.FirstOrDefault(x => x.Username == loginRequestDTO.Username &&
            //x.Password.ToLower() == loginRequestDTO.Password.ToLower());

            var user = _db.ApplicationUsers
                .FirstOrDefault(x => x.UserName.ToLower() == loginRequestDTO.Username.ToLower());


            bool isValidPass = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            //incase the user details entered does not match the details in database
            if (user == null || isValidPass == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null,
                };
            }

            //get all roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);
            //on succesfull login, generate the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            //now we define the claims for our token. Claims are nothing but will be added to token
            //as role,name, etc.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        //new Claim(ClaimTypes.Name, user.id.ToString()),
                        //new Claim(ClaimTypes.Role, user.Role)

                        //after adding identity roles will have its own table i.e. AspNetRole so 
                        //commenting above

                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role , roles.FirstOrDefault())

                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResp = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                Role = roles.FirstOrDefault()
            };
            return loginResp;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser newUser = new()
            {
                UserName = registrationRequestDTO.Username,
                Email = registrationRequestDTO.Username,
                NormalizedEmail = registrationRequestDTO.Username.ToUpper(),
                Name = registrationRequestDTO.Name
            };


            try
            {
                var result = await _userManager.CreateAsync(newUser, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "admin");
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == registrationRequestDTO.Username);
                    return _mapper.Map<UserDTO>(userToReturn);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                throw;
            }
            //await _db.LocalUsers.AddAsync(newUser);
            //await _db.SaveChangesAsync();
            
            return new UserDTO();
        }
    }
}
