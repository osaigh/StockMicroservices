using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StockMicroservices.API.Repository
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> SearchForAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(object id);
        Task<T> UpdateAsync(T entity);
    }
}
