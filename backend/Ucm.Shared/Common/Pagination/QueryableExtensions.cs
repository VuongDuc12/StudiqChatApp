
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Ucm.Shared.Common.Pagination;


namespace Ucm.Shared.Common.Pagination
{
  
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PaginationParams pagination)
        {
            // Sort
            if (!string.IsNullOrWhiteSpace(pagination.SortBy))
            {
                var sortOrder = pagination.IsDescending ? "descending" : "ascending";
                query = query.OrderBy($"{pagination.SortBy} {sortOrder}");
            }

            // Tổng số dòng
            var totalCount = await query.CountAsync();

            // Lấy dữ liệu theo trang
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }

}
