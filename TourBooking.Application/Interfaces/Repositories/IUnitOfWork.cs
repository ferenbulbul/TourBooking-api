using System;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>()
            where T : class, IBaseEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
