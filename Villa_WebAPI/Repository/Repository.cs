using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_WebAPI.Data;
using Villa_WebAPI.Models;
using Villa_WebAPI.Repository.IRepository;

namespace Villa_WebAPI.Repository
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();

            //below is to includet the Villa property to be populated when we retrieve the villa number
            //_db.VillaNumbers.Include(u => u.Villa).ToList();
            //when this property is being generated, it will be like a comma separated list
            //which we will have to seperate them using split and then include them individually 
            //like in above statement
            
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        //_db.VillaNumbers.Include(u => u.Villa).ToList();
        //when this property is being generated, it will be like a comma separated list
        //which we will have to seperate them using split and then include them individually 
        //like in above statement.
        //Eg: Villa, VillaSpecial, so we separate them
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, 
            int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (pageSize >0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                 query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        
    }
}
