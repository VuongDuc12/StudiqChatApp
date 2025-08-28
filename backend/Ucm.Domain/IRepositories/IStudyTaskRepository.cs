using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Domain.Entities;

namespace Ucm.Domain.IRepositories
{
    public interface IStudyTaskRepository: IRepositoryBase<StudyTask>
    {
        Task<IEnumerable<StudyTask>> GetByDateAsync(Guid userId, DateTime date);
        Task<IEnumerable<StudyTask>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<StudyTask>> GetByWeekAsync(Guid userId, DateTime weekStart);
        Task<IEnumerable<StudyTask>> GetByMonthAsync(Guid userId, int year, int month);
        Task<IEnumerable<StudyTask>> GetByStudyPlanIdAsync(Guid userId, int studyPlanId);
        Task<IEnumerable<StudyTask>> GetByPlanCourseIdAsync(Guid userId, int planCourseId);
        Task<IEnumerable<StudyTask>> GetUpcomingAsync(Guid userId, int days = 7);
        Task<IEnumerable<StudyTask>> GetOverdueAsync(Guid userId);
        Task<IEnumerable<StudyTask>> GetByStatusAsync(Guid userId, string status);
        Task<IEnumerable<StudyTask>> GetAllByUserIdAsync(Guid userId);
    }
}