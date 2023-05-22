﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_WebAPI.Data;
using Villa_WebAPI.Models;

namespace Villa_WebAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        //public async Task CreateAsync(Villa entity)
        //{
        //    await _db.Villas.AddAsync(entity);
        //    await SaveAsync(); 
        //}

        //public async Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        //{
        //    IQueryable<Villa> query = _db.Villas;
        //    if (!tracked)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    if (filter != null) 
        //    { 
        //        query = query.Where(filter);
        //    }
        //    return await query.FirstOrDefaultAsync();
        //}

        //public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa,bool>> filter = null)
        //{
        //    IQueryable<Villa> query = _db.Villas;
        //    if (filter != null) 
        //    { 
        //        query = query.Where(filter);
        //    }
        //    return await query.ToListAsync();
        //}

        //public async Task RemoveAsync(Villa entity)
        //{
        //    _db.Villas.Remove(entity);
        //    await SaveAsync();
        //}

        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //}

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.CreatedDate = DateTime.Now;           
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
