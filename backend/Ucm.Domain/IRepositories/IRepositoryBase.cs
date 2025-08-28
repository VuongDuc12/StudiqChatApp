using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ucm.Shared.Common.Pagination;

namespace Ucm.Domain.IRepositories
{
    public interface IRepositoryBase<T> where T : class
    {
        // Get all entities
        Task<PagedResult<T>> GetPagedAsync(PaginationParams pagination);

        Task<IEnumerable<T>> GetAllAsync();

        // Get entities with a filter
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Get a single entity by ID
        Task<T> GetByIdAsync(int id);

        // Add a new entity
        Task AddAsync(T entity);

        // Update an existing entity
        void Update(T entity);

        // Delete an entity
        void Delete(T entity);

        // Save changes (if applicable)
        Task SaveChangesAsync();
    }
}
