using AuthenticationServer.Core.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace AuthenticationServer.Core.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(Expression<Func<T, bool>> expression, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null);
        void Delete(T entity);
        Task Delete(long id);
        Task Delete(int id);
        void Delete(List<T> entities);
        Task Insert(T entity);
        void Update(T entity);
        Task<IList<T>> GetAll(List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null);
        Task<IPagedList<T>> GetPagedList(RequestParams requestParams, Expression<Func<T, bool>> expression, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<IList<T>> GetFilteredList(Expression<Func<T, bool>> expression, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    }
}
