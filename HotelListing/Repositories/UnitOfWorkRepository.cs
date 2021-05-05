using HotelListing.Contexts;
using HotelListing.Entities;
using HotelListing.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace HotelListing.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private IGenericRepository<Country> countries;
        private IGenericRepository<Hotel> hotels;
        private readonly HotelListingContext context;
        public UnitOfWorkRepository(HotelListingContext context) => this.context = context;
        public IGenericRepository<Country> Countries => countries ?? new GenericRepository<Country>(context);
        public IGenericRepository<Hotel> Hotels => hotels ?? new GenericRepository<Hotel>(context);

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task SaveAsync() => await context.SaveChangesAsync();
    }
}
