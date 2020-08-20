using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities.Base;
using Ordering.Core.Repositories.Base;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories.Base
{
    public class Repository<T>: IRepository<T> where T : Entity
    {
        protected readonly OrderContext _dbContext;

        public Repository(OrderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async  Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (disableTracking) query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();

        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool disableTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (disableTracking) query = query.AsNoTracking();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool disableTracking = true, params List<Expression<Func<T, object>>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public T Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            return entity;
        }
    }
}
