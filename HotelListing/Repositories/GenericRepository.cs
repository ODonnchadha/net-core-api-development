using HotelListing.Contexts;
using HotelListing.Interfaces.Repositories;
using HotelListing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace HotelListing.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingContext context;
        private readonly DbSet<T> db;
        public GenericRepository(HotelListingContext context)
        {
            this.context = context;
            this.db = context.Set<T>();
        }
        public async Task DeleteAsync(int id) => db.Remove(await db.FindAsync(id));
        public void DeleteRange(IEnumerable<T> entities) => db.RemoveRange(entities);

        public async Task<IList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            List<string> includes = null)
        {
            IQueryable<T> query = db;

            if (null != expression)
            {
                query = query.Where(expression);
            }

            if (null != includes)
            {
                includes.ForEach(property =>
                {
                    query = query.Include(property);
                });
            }

            if (null != orderBy)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<IPagedList<T>> GetAllAsync(
            RequestParams requestParams,
            List<string> includes = null)
        {
            IQueryable<T> query = db;

            if (null != includes)
            {
                includes.ForEach(property =>
                {
                    query = query.Include(property);
                });
            }

            return await query.AsNoTracking().ToPagedListAsync(
                requestParams.PageNumber, requestParams.PageSize);
        }
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = db;

            if (null != includes)
            {
                includes.ForEach(property =>
                {
                    query = query.Include(property);
                });
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }
        public async Task InsertAsync(T entity) => await db.AddAsync(entity);
        public async Task InsertRangeAsync(IEnumerable<T> entities) => await db.AddRangeAsync(entities);
        public void Update(T entity)
        {
            db.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
