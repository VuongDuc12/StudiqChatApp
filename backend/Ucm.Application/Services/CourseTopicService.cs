using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Shared.Common.Pagination;

namespace Ucm.Application.Services
{
    public class CourseTopicService
    {
        private readonly ICourseTopicRepository _repo;

        public CourseTopicService(ICourseTopicRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<CourseTopic>> GetPagedAsync(PaginationParams pagination)
            => await _repo.GetPagedAsync(pagination);

        public async Task<CourseTopic> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<CourseTopic>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task AddAsync(CourseTopic topic)
        {
            await _repo.AddAsync(topic);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseTopic topic)
        {
            _repo.Update(topic);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new Exception("Not found");
            _repo.Delete(existing);
            await _repo.SaveChangesAsync();
        }
    }

}
