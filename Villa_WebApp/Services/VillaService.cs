using Villa_Utility;
using Villa_WebApp.Models;
using Villa_WebApp.Models.DTO;
using Villa_WebApp.Services.IServices;

namespace Villa_WebApp.Services
{
    public class VillaNumberService : BaseService, IVillaNumberServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaUrl;

        //we are geting the value of httpClientFactory from base class BaseService and here we are
        //assigning it to _httpClientFactory
        public VillaNumberService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApiUrl");
        }


        //now we are goinf to call those APIs
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/VillaNumberAPI"

            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,                
                Url = villaUrl + "/api/VillaNumberAPI/" + id

            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberAPI"

            });
        }

        //this is caling the Send Async method in 
        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberAPI/" + id

            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/VillaNumberAPI/" + dto.VillaNo

            });
        }
    }
}
