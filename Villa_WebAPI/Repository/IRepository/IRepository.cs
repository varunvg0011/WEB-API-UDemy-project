using System.Linq.Expressions;
using Villa_WebAPI.Models;

namespace Villa_WebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);

        
    }
}
