using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task HardDeleteAsync(T entity);
        Task SoftDelete(T entity);

    }
}