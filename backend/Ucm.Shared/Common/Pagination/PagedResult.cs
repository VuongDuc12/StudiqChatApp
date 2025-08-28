using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Shared.Common.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new(); // Danh sách kết quả
        public int TotalCount { get; set; }         // Tổng số bản ghi
        public int PageNumber { get; set; }         // Trang hiện tại
        public int PageSize { get; set; }           // Số bản ghi mỗi trang

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}
