using System.Linq.Expressions;
using Villa_WebAPI.Models;
using Villa_WebAPI.Repository.IRepository;

namespace Villa_WebAPI.Repository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        //creating a generic repository named Irepository and commenting below
        //Task CreateAsync(Villa entity);
        //Task RemoveAsync(Villa entity);
        //Task SaveAsync();

        //Task<List<Villa>> GetAllAsync(Expression<Func<Villa,bool>> filter = null);
        //Task<Villa> GetAsync(Expression<Func<Villa,bool>>? filter = null, bool tracked = true);

        Task<Villa> UpdateAsync(Villa entity);
    }
}
