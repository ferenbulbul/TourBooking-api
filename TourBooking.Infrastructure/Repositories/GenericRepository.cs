using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Context;

namespace TourBooking.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDelete (T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task HardDeleteAsync(T entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
            _context.SaveChanges();
        }

    }
}