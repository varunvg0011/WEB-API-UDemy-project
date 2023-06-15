using Villa_Utility;
using Villa_WebApp.Models;
using Villa_WebApp.Models.DTO;
using Villa_WebApp.Services.IServices;

namespace Villa_WebApp.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaUrl;

        //we are geting the value of httpClientFactory from base class BaseService and here we are
        //assigning it to _httpClientFactory
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApiUrl");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = obj,
                Url = villaUrl + "/api/v1/UserAuth/login"

            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = objToCreate,
                Url = villaUrl + "/api/v1/UserAuth/register"

            });
        }
    }
}
