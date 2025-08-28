using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Ucm.Application.IServices;
using Ucm.Application.DTOs.StudyPlan;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;

namespace Ucm.Application.Services
{
    public class StudyPlanService : IStudyPlanService
    {
        private readonly IStudyPlanRepository _repository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IUserRepository _userRepository;

        public StudyPlanService(IStudyPlanRepository repository, IStudyPlanCourseRepository studyPlanCourseRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<StudyPlan>> GetAllAsync()
        {
            var plans = await _repository.GetAllAsync();
            // Đếm trực tiếp từ database cho mỗi plan
            foreach (var plan in plans)
            {
                plan.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(plan.Id);
            }
            return plans;
        }

        public async Task<IEnumerable<StudyPlan>> GetAllWithCoursesAsync()
        {
            var plans = await _repository.GetAllAsync();
            // Load courses cho mỗi plan
            foreach (var plan in plans)
            {
                var planWithCourses = await _repository.GetByIdWithCoursesAsync(plan.Id);
                if (planWithCourses != null)
                {
                    plan.PlanCourses = planWithCourses.PlanCourses;
                }
                // Đếm trực tiếp từ database
                plan.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(plan.Id);
            }
            return plans;
        }

        public async Task<IEnumerable<StudyPlan>> GetAllByUserIdAsync(Guid userId)
        {
            var plans = await _repository.GetAllByUserIdAsync(userId);
            // Đếm trực tiếp từ database cho mỗi plan
            foreach (var plan in plans)
            {
                plan.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(plan.Id);
            }
            return plans;
        }

        public async Task<StudyPlan> GetByIdAsync(int id)
        {
            var plan = await _repository.GetByIdWithCoursesAsync(id);
            if (plan != null)
            {
                System.Diagnostics.Debug.WriteLine($"GetByIdAsync: StudyPlan {id} - PlanCourses count: {plan.PlanCourses?.Count ?? 0}");
                if (plan.PlanCourses != null)
                {
                    foreach (var pc in plan.PlanCourses)
                    {
                        System.Diagnostics.Debug.WriteLine($"  - StudyPlanCourse {pc.Id}: CourseId = {pc.CourseId}");
                    }
                }
                
                // Đếm trực tiếp từ database
                plan.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(plan.Id);
            }
            return plan;
        }

        public async Task<StudyPlan> CreateAsync(StudyPlan entity)
        {
            // Đếm từ PlanCourses nếu có, hoặc 0 nếu chưa có
            entity.CourseCount = entity.PlanCourses?.Count ?? 0;
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(StudyPlan entity)
        {
            // Đếm trực tiếp từ database
            entity.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(entity.Id);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCourseCountAsync(int studyPlanId)
        {
            // Đếm trực tiếp từ database
            var courseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(studyPlanId);
            System.Diagnostics.Debug.WriteLine($"Service: StudyPlan {studyPlanId} has {courseCount} courses");
            
            // Cập nhật chỉ CourseCount mà không ảnh hưởng đến PlanCourses
            var success = await _repository.UpdateCourseCountOnlyAsync(studyPlanId, courseCount);
            
            System.Diagnostics.Debug.WriteLine($"Service: Update result = {success}");
            
            return success;
        }

        public async Task<StudyPlanUserSummaryDto> GetUserSummaryAsync(Guid userId)
        {
            // Lấy thông tin user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            // Lấy tất cả study plans của user
            var plans = await _repository.GetAllByUserIdAsync(userId);
            
            // Tính toán CourseCount cho mỗi plan
            foreach (var plan in plans)
            {
                plan.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(plan.Id);
            }

            // Phân loại plans theo trạng thái
            var activePlans = plans.Where(p => !p.Completed.HasValue || !p.Completed.Value).ToList();
            var completedPlans = plans.Where(p => p.Completed.HasValue && p.Completed.Value).ToList();
            var pendingPlans = plans.Where(p => p.StartDate > DateTime.Now).ToList();

            // Tạo summary DTOs
            var planSummaries = plans.Select(p => new StudyPlanSummaryDto
            {
                Id = p.Id,
                PlanName = p.PlanName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Semester = p.Semester,
                AcademicYear = p.AcademicYear,
                CourseCount = p.CourseCount,
                Completed = p.Completed,
                Status = GetPlanStatus(p)
            }).ToList();

            return new StudyPlanUserSummaryDto
            {
                UserId = user.Id,
                UserName = user.Username,
                UserEmail = user.Email,
                TotalPlans = plans.Count(),
                ActivePlans = activePlans.Count,
                CompletedPlans = completedPlans.Count,
                PendingPlans = pendingPlans.Count,
                Plans = planSummaries
            };
        }

        private string GetPlanStatus(StudyPlan plan)
        {
            if (plan.Completed.HasValue && plan.Completed.Value)
                return "Completed";
            
            if (plan.StartDate > DateTime.Now)
                return "Pending";
            
            return "Active";
        }

        public async Task<StudyPlanAdminSummaryDto> GetAdminSummaryAsync()
        {
            // Lấy tất cả users
            var users = await _userRepository.GetAllAsync();
            var userSummaries = new List<UserPlanSummaryDto>();
            
            int totalPlans = 0;
            int totalActivePlans = 0;
            int totalCompletedPlans = 0;
            int totalPendingPlans = 0;

            foreach (var user in users)
            {
                // Lấy tất cả study plans của user
                var plans = await _repository.GetAllByUserIdAsync(user.Id);
                
                // Tính toán CourseCount cho mỗi plan
                foreach (var plan in plans)
                {
                    plan.CourseCount = await _studyPlanCourseRepository.CountByStudyPlanIdAsync(plan.Id);
                }

                // Phân loại plans theo trạng thái
                var activePlans = plans.Where(p => !p.Completed.HasValue || !p.Completed.Value).ToList();
                var completedPlans = plans.Where(p => p.Completed.HasValue && p.Completed.Value).ToList();
                var pendingPlans = plans.Where(p => p.StartDate > DateTime.Now).ToList();

                // Tạo summary DTOs cho plans
                var planSummaries = plans.Select(p => new StudyPlanSummaryDto
                {
                    Id = p.Id,
                    PlanName = p.PlanName,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Semester = p.Semester,
                    AcademicYear = p.AcademicYear,
                    CourseCount = p.CourseCount,
                    Completed = p.Completed,
                    Status = GetPlanStatus(p)
                }).ToList();

                var userSummary = new UserPlanSummaryDto
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    UserEmail = user.Email,
                    TotalPlans = plans.Count(),
                    ActivePlans = activePlans.Count,
                    CompletedPlans = completedPlans.Count,
                    PendingPlans = pendingPlans.Count,
                    Plans = planSummaries
                };

                userSummaries.Add(userSummary);

                // Cộng dồn tổng số
                totalPlans += plans.Count();
                totalActivePlans += activePlans.Count;
                totalCompletedPlans += completedPlans.Count;
                totalPendingPlans += pendingPlans.Count;
            }

            return new StudyPlanAdminSummaryDto
            {
                TotalUsers = users.Count(),
                TotalPlans = totalPlans,
                TotalActivePlans = totalActivePlans,
                TotalCompletedPlans = totalCompletedPlans,
                TotalPendingPlans = totalPendingPlans,
                Users = userSummaries
            };
        }
    }
}