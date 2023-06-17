using System.Linq.Expressions;
using Villa_WebAPI.Models;

namespace Villa_WebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();

        //here we are adding the third property to let it know that hey, we need to include
        //this property as well.
        //adding pageNo and pageSize for pagination
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties= null,
            int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);

        
    }
}
