using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities;

namespace Ucm.Domain.IRepositories
{
    public interface IStudyPlanCourseRepository : IRepositoryBase<StudyPlanCourse>
    {
        Task<IEnumerable<StudyPlanCourse>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<StudyPlanCourse>> GetByStudyPlanIdAsync(int studyPlanId);
        Task<int> CountByStudyPlanIdAsync(int studyPlanId);
        Task<StudyPlanCourse> GetByIdWithCourseAsync(int id);
        Task<IEnumerable<StudyPlanCourse>> GetAllByUserIdAsync(Guid userId);
    }
}