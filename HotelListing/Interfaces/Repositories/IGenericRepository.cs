using HotelListing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace HotelListing.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        // Create
        Task InsertAsync(T entity);
        Task InsertRangeAsync(IEnumerable<T> entities);
        // Read
        Task<IList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null);
        Task<IPagedList<T>> GetAllAsync(
                        RequestParams requestParams,
            List<string> includes = null);
        Task<T> GetOneAsync(
            Expression<Func<T, bool>> expression = null,
            List<string> includes = null);
        // Update
        void Update(T entity);
        // Delete
        Task DeleteAsync(int id);
        void DeleteRange(IEnumerable<T> entities);
    }
}
