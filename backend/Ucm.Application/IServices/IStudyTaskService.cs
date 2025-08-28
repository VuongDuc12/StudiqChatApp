using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Application.Dtos;

namespace Ucm.Application.IServices
{
    public interface IStudyTaskService
    {
        Task<IEnumerable<StudyTaskDto>> GetAllAsync(Guid userId);
        Task<StudyTaskDto> GetByIdAsync(int id);
        Task<StudyTaskDto> AddAsync(CreateStudyTaskRequest request);
        Task UpdateAsync(int id, UpdateStudyTaskRequest request);
        Task DeleteAsync(int id);
        
        // Mobile app specific methods
        Task<IEnumerable<StudyTaskDto>> GetByDateAsync(Guid userId, DateTime date);
        Task<IEnumerable<StudyTaskDto>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<StudyTaskDto>> GetByWeekAsync(Guid userId, DateTime weekStart);
        Task<IEnumerable<StudyTaskDto>> GetByMonthAsync(Guid userId, int year, int month);
        Task<IEnumerable<StudyTaskDto>> GetByStudyPlanIdAsync(Guid userId, int studyPlanId);
        Task<IEnumerable<StudyTaskDto>> GetByPlanCourseIdAsync(Guid userId, int planCourseId);
        Task<IEnumerable<StudyTaskDto>> GetTodayAsync(Guid userId);
        Task<IEnumerable<StudyTaskDto>> GetUpcomingAsync(Guid userId, int days = 7);
        Task<IEnumerable<StudyTaskDto>> GetOverdueAsync(Guid userId);
        Task<IEnumerable<StudyTaskDto>> GetByStatusAsync(Guid userId, string status);
    }
}