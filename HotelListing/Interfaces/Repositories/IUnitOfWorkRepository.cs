using HotelListing.Entities;
using System;
using System.Threading.Tasks;

namespace HotelListing.Interfaces.Repositories
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task SaveAsync();
    }
}
