using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Application.DTOs.StudyPlan;

namespace Ucm.Application.IServices
{
    public interface IStudyPlanService
    {
        Task<IEnumerable<StudyPlan>> GetAllAsync();
        Task<IEnumerable<StudyPlan>> GetAllWithCoursesAsync();
        Task<IEnumerable<StudyPlan>> GetAllByUserIdAsync(Guid userId);
        Task<StudyPlan> GetByIdAsync(int id);
        Task<StudyPlan> CreateAsync(StudyPlan entity);
        Task<bool> UpdateAsync(StudyPlan entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateCourseCountAsync(int studyPlanId);
        Task<StudyPlanUserSummaryDto> GetUserSummaryAsync(Guid userId);
        Task<StudyPlanAdminSummaryDto> GetAdminSummaryAsync();
    }
}