using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Business.Abstract
{
    public interface IGenericService<T>
    {
        Task<T> GetAsync(Expression<Func<T,bool>>Filter , params string[] IncludeParameters);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Filter=null, params string[] IncludeParameters);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
