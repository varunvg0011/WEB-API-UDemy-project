using System.Linq.Expressions;
using Villa_WebAPI.Models;
using Villa_WebAPI.Repository.IRepository;

namespace Villa_WebAPI.Repository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        

        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}
