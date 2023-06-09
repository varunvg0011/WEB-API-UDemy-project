using Villa_WebApp.Models.DTO;

namespace Villa_WebApp.Services.IServices
{
    //defining an xplicit service for Villa Controller to call the API
    public interface IVillaNumberServices
    {
        //we are dding the string token later in the project in order to call the API 
        //so that we are able to access the APIs as some needs authorization
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
