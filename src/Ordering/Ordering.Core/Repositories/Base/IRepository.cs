using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ordering.Core.Entities.Base;

namespace Ordering.Core.Repositories.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true
        );

        Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = true,
            params List<Expression<Func<T, object>>>[] includes
        );

        Task<T> GetByIdAsync(Guid id);

        Task<T> AddAsync(T entity);

        T Update(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();

    }
}
