using Villa_WebApp.Models.DTO;

namespace Villa_WebApp.Services.IServices
{
    //defining an xplicit service for Villa Controller to call the API
    public interface IVillaNumberServices
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO dto);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
