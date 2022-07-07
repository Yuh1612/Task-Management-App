﻿using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<T> dbSet => _dbContext.Set<T>();

        public async Task Remove(int id)
        {
            var entity = await FindAsync(id);
            if (entity != null) Remove(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public async Task<T?> FindAsync(params object[] keyValues)
        {
            var result = await dbSet.FindAsync(keyValues);
            if (result == null) throw new KeyNotFoundException();
            return result;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}