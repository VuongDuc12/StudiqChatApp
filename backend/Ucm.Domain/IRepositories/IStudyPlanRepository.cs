using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities;

namespace Ucm.Domain.IRepositories
{
    public interface IStudyPlanRepository : IRepositoryBase<StudyPlan>
    {
        // Thêm các method đặc thù nếu cần, ví dụ:
        Task<IEnumerable<StudyPlan>> GetAllByUserIdAsync(Guid userId);
        Task<IEnumerable<StudyPlan>> GetByIdsAsync(List<int> ids);
        Task<StudyPlan> GetByIdWithCoursesAsync(int id);
        Task<bool> UpdateCourseCountOnlyAsync(int studyPlanId, int courseCount);
    }
}