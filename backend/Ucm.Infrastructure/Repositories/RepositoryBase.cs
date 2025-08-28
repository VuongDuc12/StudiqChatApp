using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Shared.Common.Pagination;

namespace HotelApp.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity, TEf> : IRepositoryBase<TEntity>
        where TEntity : class
        where TEf : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEf> _dbSet;
        protected readonly IEntityEfMapper<TEntity, TEf> _mapper;

        public RepositoryBase(AppDbContext context, IEntityEfMapper<TEntity, TEf> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEf>();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<PagedResult<TEntity>> GetPagedAsync(PaginationParams pagination)
        {
            IQueryable<TEf> query = _dbSet;

            // Apply basic search filter (nếu có)
            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
            {
                query = ApplySearchFilter(query, pagination.SearchTerm);
            }

            var pagedEf = await query.ToPagedResultAsync(pagination);

            return new PagedResult<TEntity>
            {
                Items = pagedEf.Items.Select(_mapper.ToEntity).ToList(),
                TotalCount = pagedEf.TotalCount,
                PageNumber = pagedEf.PageNumber,
                PageSize = pagedEf.PageSize
            };
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var efList = await _dbSet.ToListAsync();
            return efList.Select(e => _mapper.ToEntity(e));
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            // Not directly supported; you may need to map predicate or filter in-memory
            var efList = await _dbSet.ToListAsync();
            var entities = efList.Select(e => _mapper.ToEntity(e));
            return entities.Where(predicate.Compile());
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var ef = await _dbSet.FindAsync(id);
            return ef == null ? null : _mapper.ToEntity(ef);
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            Console.WriteLine($"Converting domain entity to EF entity...");
            var ef = _mapper.ToEf(entity);
            Console.WriteLine($"Adding EF entity to DbSet...");
            await _dbSet.AddAsync(ef);

            // Lưu ngay để lấy Id
            await _context.SaveChangesAsync();
            Console.WriteLine($"Saved changes to get Id. EF entity Id: {ef.GetType().GetProperty("Id")?.GetValue(ef)}");

            // Gán lại Id cho entity domain
            var idProp = typeof(TEntity).GetProperty("Id");
            var efIdProp = ef.GetType().GetProperty("Id");
            if (idProp != null && efIdProp != null)
            {
                var efId = efIdProp.GetValue(ef);
                idProp.SetValue(entity, efId);
                Console.WriteLine($"Set domain entity Id to: {efId}");
            }
        }

        public void Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _mapper.ToEf(entity);
            _dbSet.Update(ef);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            // Lấy Id từ entity domain
            var idProp = typeof(TEntity).GetProperty("Id");
            if (idProp == null) throw new InvalidOperationException("Entity must have Id property");
            var id = idProp.GetValue(entity);

            // Tìm instance EF đang được tracking hoặc trong database
            var ef = _dbSet.Find(id);
            if (ef == null)
            {
                // Nếu chưa được tracking, map sang EF và attach vào context
                ef = _mapper.ToEf(entity);
                _dbSet.Attach(ef);
            }
            _dbSet.Remove(ef);
        }

        public async Task SaveChangesAsync()
        {
            Console.WriteLine("Saving additional changes to database...");
            await _context.SaveChangesAsync();
            Console.WriteLine("Additional changes saved successfully.");
        }
        protected virtual IQueryable<TEf> ApplySearchFilter(IQueryable<TEf> query, string searchTerm)
        {
            return query; // Mặc định: không lọc gì cả
        }
    }
}
