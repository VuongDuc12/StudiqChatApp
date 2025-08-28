using System;

namespace Ucm.Shared.Common.Pagination
{
    public class PaginationParams
    {
        private const int MaxPageSize = 100;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? SortBy { get; set; } // tên property trong entity
        public bool IsDescending { get; set; } = false;
        public string? SearchTerm { get; set; } // bộ lọc cơ bản
    }
}
