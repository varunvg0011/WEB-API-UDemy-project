using AutoMapper;
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
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db= db;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("AppSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.Username == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.Username == loginRequestDTO.Username &&
            x.Password.ToLower() == loginRequestDTO.Password.ToLower());

            //incase the user details entered does not match the details in database
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = user,
                };
            }

            //on succesfull login, generate the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            //now we define the claims for our token. Claims are nothing but will be added to token
            //as role,name, etc.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResp = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };
            return loginResp;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser newUser = _mapper.Map<LocalUser>(registrationRequestDTO);
            await _db.LocalUsers.AddAsync(newUser);
            await _db.SaveChangesAsync();
            newUser.Password = "";
            return newUser;
        }
    }
}
