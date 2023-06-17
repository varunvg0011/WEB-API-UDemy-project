using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;

namespace Villa_WebAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        //Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
