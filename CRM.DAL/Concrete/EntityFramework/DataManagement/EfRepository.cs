using Azure.Core;
using CRM.DAL.Abstract.DataManagement;
using CRM.Entity.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Concrete.EntityFramework.DataManagement
{
    public class EfRepository<T> : IRepository<T> where T : AuditableEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> Filter, params string[] IncludeParameters)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(Filter);

            if (IncludeParameters.Length > 0)
            {
                foreach (var includeItem in IncludeParameters)
                {
                    query = query.Include(includeItem);
                }
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Filter = null, params string[] IncludeParameters)
        {
            IQueryable<T> query = _dbSet;

            if (Filter is not null)
            {
                query = query.Where(Filter);
            }

            if (IncludeParameters.Length > 0)
            {
                foreach (var includeItem in IncludeParameters)
                {
                    query = query.Include(includeItem);
                }
            }
            return await Task.Run(() => query);
        }

        public async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => _dbSet.Update(entity));
        }
    }
}
