using AuthenticationServer.Core.IRepository;
using AuthenticationServer.Core.Models;
using AuthenticationServer.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;


namespace AuthenticationServer.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataBaseContext _context;
        private DbSet<T> _table;

        public GenericRepository(DataBaseContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _table.FindAsync(id);
            Delete(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _table.FindAsync(id);
            Delete(entity);
        }

        public void Delete(List<T> entities)
        {
            _table.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null)
        {
            IQueryable<T> query = _table;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }
            return await query.AsNoTracking().SingleOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null)
        {
            IQueryable<T> query = _table;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IList<T>> GetFilteredList(Expression<Func<T, bool>> expression, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _table;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<IPagedList<T>> GetPagedList(RequestParams requestParams, Expression<Func<T, bool>> expression, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IEnumerable<string> values = new List<string>() { "Sahand", "Ali" };
            IQueryable<T> query = _table;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().Where(expression).ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
        }

        public async Task Insert(T entity)
        {
            await _table.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
