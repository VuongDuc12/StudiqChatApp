using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Application.Dtos;
using Ucm.Application.DTOs;
using Ucm.Application.IServices;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using System;

namespace Ucm.Application.Services
{
    public class StudyTaskService : IStudyTaskService
    {
        private readonly IStudyTaskRepository _repository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTopicRepository _courseTopicRepository;

        public StudyTaskService(
            IStudyTaskRepository repository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyPlanRepository studyPlanRepository,
            ICourseRepository courseRepository,
            ICourseTopicRepository courseTopicRepository)
        {
            _repository = repository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _studyPlanRepository = studyPlanRepository;
            _courseRepository = courseRepository;
            _courseTopicRepository = courseTopicRepository;
        }

        private async Task<StudyTaskDto> MapToDtoWithDetails(StudyTask task)
        {
            try
            {
                Console.WriteLine($"Mapping task ID: {task?.Id}");
                Console.WriteLine($"Task PlanCourseId: {task?.PlanCourseId}");
                Console.WriteLine($"Task PlanCourse: {task?.PlanCourse != null}");
                Console.WriteLine($"Task PlanCourse.StudyPlan: {task?.PlanCourse?.StudyPlan != null}");
                Console.WriteLine($"Task PlanCourse.Course: {task?.PlanCourse?.Course != null}");

                // Since repository now loads related data, we can use it directly
                return new StudyTaskDto
                {
                    Id = task.Id,
                    PlanCourseId = task.PlanCourseId,
                    StudyPlanId = task.PlanCourse?.StudyPlan?.Id ?? 0,
                    StudyPlanName = task.PlanCourse?.StudyPlan?.PlanName ?? "Unknown",
                    CourseId = task.PlanCourse?.Course?.Id ?? 0,
                    CourseName = task.PlanCourse?.Course?.CourseName ?? "Unknown",
                    CourseTopicId = task.CourseTopicId,
                    CourseTopicName = task.CourseTopic?.TopicName,
                    TaskName = task.TaskName ?? "",
                    TaskDescription = task.TaskDescription ?? "",
                    EstimatedHours = task.EstimatedHours,
                    DueDate = task.DueDate,
                    ScheduledDate = task.ScheduledDate,
                    Status = task.Status ?? "Unknown",
                    CompletionDate = task.CompletionDate,
                    Logs = task.Logs?.Select(log => new StudyLogDto
                    {
                        Id = log.Id,
                        TaskId = log.TaskId,
                        ActualTimeSpent = log.ActualTimeSpent,
                        LogDate = log.LogDate
                    }).ToList() ?? new List<StudyLogDto>(),
                    Resources = task.Resources?.Select(res => new TaskResourceDto
                    {
                        Id = res.Id,
                        TaskId = res.TaskId,
                        ResourceType = res.ResourceType,
                        ResourceURL = res.ResourceURL
                    }).ToList() ?? new List<TaskResourceDto>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MapToDtoWithDetails: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetAllAsync(Guid userId)
        {
            try
            {
                Console.WriteLine($"Service: Getting all tasks for user: {userId}");
                var entities = await _repository.GetAllByUserIdAsync(userId);
                Console.WriteLine($"Service: Found {entities.Count()} tasks for user.");

                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudyTaskDto> GetByIdAsync(int id)
        {
            try
            {
                Console.WriteLine($"Getting task by ID: {id}");
                var entity = await _repository.GetByIdAsync(id);
                Console.WriteLine($"Entity found: {entity != null}");
                return entity == null ? null : await MapToDtoWithDetails(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<StudyTaskDto> AddAsync(CreateStudyTaskRequest request)
        {
            try
            {
                Console.WriteLine($"Adding task with PlanCourseId: {request.PlanCourseId}");
                Console.WriteLine($"Request CourseTopicId: {request.CourseTopicId}");

                // Validate PlanCourseId belongs to current user
                var planCourse = await _studyPlanCourseRepository.GetByIdAsync(request.PlanCourseId);
                Console.WriteLine($"PlanCourse found: {planCourse != null}");
                
                if (planCourse != null)
                {
                    Console.WriteLine($"PlanCourse details - Id: {planCourse.Id}, StudyPlanId: {planCourse.StudyPlanId}, CourseId: {planCourse.CourseId}");
                }
                
                if (planCourse == null)
                    throw new ArgumentException("Invalid PlanCourseId - PlanCourse not found");

                // Validate PlanCourse has valid StudyPlanId and CourseId
                if (planCourse.StudyPlanId <= 0 || planCourse.CourseId <= 0)
                    throw new ArgumentException("Invalid PlanCourse - StudyPlanId or CourseId is invalid");

                // Validate CourseTopicId if provided
                int? courseTopicId = null;
                if (request.CourseTopicId.HasValue && request.CourseTopicId.Value > 0)
                {
                    Console.WriteLine($"Validating CourseTopicId: {request.CourseTopicId.Value}");
                    var courseTopic = await _courseTopicRepository.GetByIdAsync(request.CourseTopicId.Value);
                    Console.WriteLine($"CourseTopic found: {courseTopic != null}");
                    
                    if (courseTopic == null)
                        throw new ArgumentException($"Invalid CourseTopicId - CourseTopic with ID {request.CourseTopicId.Value} not found");
                    courseTopicId = request.CourseTopicId.Value;
                }

                var entity = new StudyTask
                {
                    PlanCourseId = request.PlanCourseId,
                    CourseTopicId = request.CourseTopicId,
                    TaskName = request.TaskName,
                    TaskDescription = request.TaskDescription,
                    EstimatedHours = request.EstimatedHours,
                    DueDate = request.DueDate,
                    ScheduledDate = request.ScheduledDate,
                    Status = request.Status
                };

                Console.WriteLine($"Creating task with PlanCourseId: {entity.PlanCourseId}");
                await _repository.AddAsync(entity);  // This will also save changes and set the Id
                Console.WriteLine($"Task created and saved with Id: {entity.Id}");

                // Lấy lại task với đầy đủ related data
                var createdEntity = await _repository.GetByIdAsync(entity.Id);
                Console.WriteLine($"Loaded created task: {createdEntity != null}, Id: {createdEntity?.Id}");

                if (createdEntity == null)
                {
                    throw new Exception($"Task was saved with Id {entity.Id} but could not be loaded from DB");
                }

                return await MapToDtoWithDetails(createdEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        
        public async Task UpdateAsync(int id, UpdateStudyTaskRequest request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    throw new ArgumentException("Task not found");

                // Validate CourseTopicId if provided
                if (request.CourseTopicId.HasValue && request.CourseTopicId.Value > 0)
                {
                    var courseTopic = await _courseTopicRepository.GetByIdAsync(request.CourseTopicId.Value);
                    if (courseTopic == null)
                        throw new ArgumentException($"Invalid CourseTopicId - CourseTopic with ID {request.CourseTopicId.Value} not found");
                    entity.CourseTopicId = request.CourseTopicId.Value;
                }
                else if (request.CourseTopicId.HasValue && request.CourseTopicId.Value == 0)
                {
                    // Set to null if explicitly set to 0
                    entity.CourseTopicId = null;
                }

                // Chỉ cập nhật những trường được cung cấp
                if (!string.IsNullOrEmpty(request.TaskName))
                    entity.TaskName = request.TaskName;
                
                if (!string.IsNullOrEmpty(request.TaskDescription))
                    entity.TaskDescription = request.TaskDescription;
                
                if (request.EstimatedHours.HasValue)
                    entity.EstimatedHours = request.EstimatedHours;
                
                if (request.DueDate.HasValue)
                    entity.DueDate = request.DueDate;
                
                if (request.ScheduledDate.HasValue)
                    entity.ScheduledDate = request.ScheduledDate;
                
                if (!string.IsNullOrEmpty(request.Status))
                    entity.Status = request.Status;
                
                if (request.CompletionDate.HasValue)
                    entity.CompletionDate = request.CompletionDate;

                _repository.Update(entity);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return;
                _repository.Delete(entity);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }

        // Mobile app specific methods
        public async Task<IEnumerable<StudyTaskDto>> GetByDateAsync(Guid userId, DateTime date)
        {
            try
            {
                var entities = await _repository.GetByDateAsync(userId, date);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByDateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var entities = await _repository.GetByDateRangeAsync(userId, startDate, endDate);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByDateRangeAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetByWeekAsync(Guid userId, DateTime weekStart)
        {
            try
            {
                var entities = await _repository.GetByWeekAsync(userId, weekStart);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByWeekAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetByMonthAsync(Guid userId, int year, int month)
        {
            try
            {
                var entities = await _repository.GetByMonthAsync(userId, year, month);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByMonthAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetByStudyPlanIdAsync(Guid userId, int studyPlanId)
        {
            try
            {
                var entities = await _repository.GetByStudyPlanIdAsync(userId, studyPlanId);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByStudyPlanIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetByPlanCourseIdAsync(Guid userId, int planCourseId)
        {
            try
            {
                var entities = await _repository.GetByPlanCourseIdAsync(userId, planCourseId);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByPlanCourseIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetTodayAsync(Guid userId)
        {
            try
            {
                return await GetByDateAsync(userId, DateTime.Today);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTodayAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetUpcomingAsync(Guid userId, int days = 7)
        {
            try
            {
                var entities = await _repository.GetUpcomingAsync(userId, days);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUpcomingAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetOverdueAsync(Guid userId)
        {
            try
            {
                var entities = await _repository.GetOverdueAsync(userId);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOverdueAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<StudyTaskDto>> GetByStatusAsync(Guid userId, string status)
        {
            try
            {
                var entities = await _repository.GetByStatusAsync(userId, status);
                var dtos = new List<StudyTaskDto>();
                foreach (var entity in entities)
                {
                    dtos.Add(await MapToDtoWithDetails(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByStatusAsync: {ex.Message}");
                throw;
            }
        }
    }
}