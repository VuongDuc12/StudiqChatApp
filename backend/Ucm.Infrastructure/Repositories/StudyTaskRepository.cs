using HotelApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.Enums;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Repositories
{
    public class StudyTaskRepository : RepositoryBase<StudyTask, StudyTaskEf>, IStudyTaskRepository
    {
       
        public StudyTaskRepository(AppDbContext context, IEntityEfMapper<StudyTask, StudyTaskEf> mapper)
          : base(context, mapper) { }

        // Helper method to include all related data
        private IQueryable<StudyTaskEf> IncludeAllRelatedData(IQueryable<StudyTaskEf> query)
        {
            Console.WriteLine("Including all related data for StudyTask...");
            var result = query
                .Include(t => t.PlanCourse)
                    .ThenInclude(pc => pc.StudyPlan)
                .Include(t => t.PlanCourse)
                    .ThenInclude(pc => pc.Course)
                .Include(t => t.CourseTopic)
                .Include(t => t.Logs)
                .Include(t => t.Resources)
                .AsSplitQuery(); // Use split query for better performance with multiple includes

            Console.WriteLine("Related data included successfully.");
            return result;
        }

        // Override GetByIdAsync to include related data
        public new async Task<StudyTask> GetByIdAsync(int id)
        {
            Console.WriteLine($"StudyTaskRepository.GetByIdAsync - Loading task with Id: {id}");
            var query = IncludeAllRelatedData(_context.StudyTasks);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);
            
            if (entity != null)
            {
                Console.WriteLine($"Found task - Id: {entity.Id}, PlanCourseId: {entity.PlanCourseId}");
                Console.WriteLine($"PlanCourse loaded: {entity.PlanCourse != null}");
                if (entity.PlanCourse != null)
                {
                    Console.WriteLine($"PlanCourse details - Id: {entity.PlanCourse.Id}, StudyPlanId: {entity.PlanCourse.StudyPlanId}, CourseId: {entity.PlanCourse.CourseId}");
                    Console.WriteLine($"StudyPlan loaded: {entity.PlanCourse.StudyPlan != null}, Course loaded: {entity.PlanCourse.Course != null}");
                    if (entity.PlanCourse.StudyPlan != null)
                    {
                        Console.WriteLine($"StudyPlan details - Id: {entity.PlanCourse.StudyPlan.Id}, Name: {entity.PlanCourse.StudyPlan.PlanName}");
                    }
                    if (entity.PlanCourse.Course != null)
                    {
                        Console.WriteLine($"Course details - Id: {entity.PlanCourse.Course.Id}, Name: {entity.PlanCourse.Course.CourseName}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No task found with Id: {id}");
            }

            var result = entity != null ? _mapper.ToEntity(entity) : null;
            if (result != null)
            {
                Console.WriteLine($"Mapped to domain entity - Id: {result.Id}, PlanCourseId: {result.PlanCourseId}");
                Console.WriteLine($"Domain PlanCourse: {result.PlanCourse != null}");
                if (result.PlanCourse != null)
                {
                    Console.WriteLine($"Domain StudyPlan: {result.PlanCourse.StudyPlan != null}, Course: {result.PlanCourse.Course != null}");
                }
            }

            return result;
        }

        public async Task<IEnumerable<StudyTask>> GetByDateAsync(Guid userId, DateTime date)
        {
            var planCourseIdsForUser = await _context.StudyPlanCourses
                                                     .Where(spc => spc.UserId == userId)
                                                     .Select(spc => spc.Id)
                                                     .ToListAsync();

            if (!planCourseIdsForUser.Any())
            {
                return new List<StudyTask>();
            }

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                .Where(x => x.ScheduledDate.HasValue && 
                           x.ScheduledDate.Value == DateOnly.FromDateTime(date) && 
                           planCourseIdsForUser.Contains(x.PlanCourseId))
                .OrderBy(x => x.ScheduledDate)
                .ThenBy(x => x.DueDate)
                .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyTask>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var planCourseIdsForUser = await _context.StudyPlanCourses
                                                     .Where(spc => spc.UserId == userId)
                                                     .Select(spc => spc.Id)
                                                     .ToListAsync();

            if (!planCourseIdsForUser.Any())
            {
                return new List<StudyTask>();
            }

            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                .Where(x => x.ScheduledDate.HasValue && 
                           x.ScheduledDate.Value >= startDateOnly && 
                           x.ScheduledDate.Value <= endDateOnly && 
                           planCourseIdsForUser.Contains(x.PlanCourseId))
                .OrderBy(x => x.ScheduledDate)
                .ThenBy(x => x.DueDate)
                .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyTask>> GetByWeekAsync(Guid userId, DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(6);
            return await GetByDateRangeAsync(userId, weekStart, weekEnd);
        }

        public async Task<IEnumerable<StudyTask>> GetByMonthAsync(Guid userId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return await GetByDateRangeAsync(userId, startDate, endDate);
        }

        public async Task<IEnumerable<StudyTask>> GetByStudyPlanIdAsync(Guid userId, int studyPlanId)
        {
            var planCourseIds = await _context.StudyPlanCourses
                                              .Where(spc => spc.StudyPlanId == studyPlanId && spc.UserId == userId)
                                              .Select(spc => spc.Id)
                                              .ToListAsync();

            if (!planCourseIds.Any())
            {
                return new List<StudyTask>();
            }

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                .Where(x => planCourseIds.Contains(x.PlanCourseId))
                .OrderBy(x => x.ScheduledDate)
                .ThenBy(x => x.DueDate)
                .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyTask>> GetByPlanCourseIdAsync(Guid userId, int planCourseId)
        {
            // Verify user has access to this plan course
            var hasAccess = await _context.StudyPlanCourses
                .AnyAsync(spc => spc.Id == planCourseId && spc.UserId == userId);

            if (!hasAccess)
            {
                return new List<StudyTask>();
            }

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                .Where(x => x.PlanCourseId == planCourseId)
                .OrderBy(x => x.ScheduledDate)
                .ThenBy(x => x.DueDate)
                .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyTask>> GetUpcomingAsync(Guid userId, int days = 7)
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(days);
            return await GetByDateRangeAsync(userId, startDate, endDate);
        }

        public async Task<IEnumerable<StudyTask>> GetOverdueAsync(Guid userId)
        {
            var planCourseIdsForUser = await _context.StudyPlanCourses
                                                     .Where(spc => spc.UserId == userId)
                                                     .Select(spc => spc.Id)
                                                     .ToListAsync();

            if (!planCourseIdsForUser.Any())
            {
                return new List<StudyTask>();
            }

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                .Where(x => x.DueDate.HasValue && 
                           x.DueDate.Value < DateTime.Now && 
                           x.Status != "Completed" &&
                           planCourseIdsForUser.Contains(x.PlanCourseId))
                .OrderBy(x => x.DueDate)
                .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyTask>> GetByStatusAsync(Guid userId, string status)
        {
            var planCourseIdsForUser = await _context.StudyPlanCourses
                                                     .Where(spc => spc.UserId == userId)
                                                     .Select(spc => spc.Id)
                                                     .ToListAsync();

            if (!planCourseIdsForUser.Any())
            {
                return new List<StudyTask>();
            }

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                .Where(x => x.Status == status && 
                           planCourseIdsForUser.Contains(x.PlanCourseId))
                .OrderBy(x => x.ScheduledDate)
                .ThenBy(x => x.DueDate)
                .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyTask>> GetAllByUserIdAsync(Guid userId)
        {
            var userPlanCourseIds = await _context.StudyPlanCourses
                                                  .Where(spc => spc.UserId == userId)
                                                  .Select(spc => spc.Id)
                                                  .ToListAsync();

            if (!userPlanCourseIds.Any())
            {
                return Enumerable.Empty<StudyTask>();
            }

            var entities = await IncludeAllRelatedData(_context.StudyTasks)
                                 .Where(t => userPlanCourseIds.Contains(t.PlanCourseId))
                                 .ToListAsync();

            return entities.Select(_mapper.ToEntity);
        }
    }
}
