using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using System.Linq;
using Ucm.Domain.Enums;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class StudyPlanCourseEntityEfMapper : IEntityEfMapper<StudyPlanCourse, StudyPlanCourseEf>
    {
        public StudyPlanCourse ToEntity(StudyPlanCourseEf ef)
        {
            if (ef == null) return null;

            return new StudyPlanCourse
            {
                Id = ef.Id,
                StudyPlanId = ef.StudyPlanId,
                StudyPlan = ef.StudyPlan != null ? MapStudyPlan(ef.StudyPlan) : null,
                CourseId = ef.CourseId,
                Course = ef.Course != null ? MapCourse(ef.Course) : null,
                UserId = ef.UserId,
                Tasks = ef.Tasks?.Select(MapStudyTask).ToList()
            };
        }

        public StudyPlanCourseEf ToEf(StudyPlanCourse entity)
        {
            if (entity == null) return null;

            return new StudyPlanCourseEf
            {
                Id = entity.Id,
                StudyPlanId = entity.StudyPlanId,
                StudyPlan = entity.StudyPlan != null ? MapStudyPlanToEf(entity.StudyPlan) : null,
                CourseId = entity.CourseId,
                Course = entity.Course != null ? MapCourseToEf(entity.Course) : null,
                UserId = entity.UserId,
                Tasks = entity.Tasks?.Select(MapStudyTaskToEf).ToList()
            };
        }

        // Helper methods to avoid circular dependency
        private StudyPlan MapStudyPlan(StudyPlanEf ef)
        {
            return new StudyPlan
            {
                Id = ef.Id,
                PlanName = ef.PlanName,
                StartDate = ef.StartDate,
                EndDate = ef.EndDate,
                Semester = ef.Semester,
                AcademicYear = ef.AcademicYear,
                WeeklyStudyGoalHours = ef.WeeklyStudyGoalHours,
                CourseCount = ef.CourseCount,
                Completed = ef.Completed,
                UserId = ef.UserId,
                PlanCourses = null // Avoid circular reference
            };
        }

        private StudyPlanEf MapStudyPlanToEf(StudyPlan entity)
        {
            return new StudyPlanEf
            {
                Id = entity.Id,
                PlanName = entity.PlanName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Semester = entity.Semester,
                AcademicYear = entity.AcademicYear,
                WeeklyStudyGoalHours = entity.WeeklyStudyGoalHours,
                CourseCount = entity.CourseCount,
                Completed = entity.Completed,
                UserId = entity.UserId,
                PlanCourses = null // Avoid circular reference
            };
        }

        private Course MapCourse(CourseEf ef)
        {
            return new Course
            {
                Id = ef.Id,
                CourseName = ef.CourseName,
                Description = ef.Description,
                Credits = ef.Credits,
                Topics = null // Avoid circular reference
            };
        }

        private CourseEf MapCourseToEf(Course entity)
        {
            return new CourseEf
            {
                Id = entity.Id,
                CourseName = entity.CourseName,
                Description = entity.Description,
                Credits = entity.Credits,
                Topics = null // Avoid circular reference
            };
        }

        private StudyTask MapStudyTask(StudyTaskEf ef)
        {
            return new StudyTask
            {
                Id = ef.Id,
                PlanCourseId = ef.PlanCourseId,
                CourseTopicId = ef.CourseTopicId,
                TaskName = ef.TaskName,
                TaskDescription = ef.TaskDescription,
                EstimatedHours = ef.EstimatedHours,
                DueDate = ef.DueDate,
                ScheduledDate = ef.ScheduledDate,
                Status = ef.Status,
                CompletionDate = ef.CompletionDate,
                PlanCourse = null, // Avoid circular reference
                CourseTopic = null, // Avoid circular reference
                Logs = ef.Logs?.Select(MapStudyLog).ToList(),
                Resources = ef.Resources?.Select(MapTaskResource).ToList()
            };
        }

        private StudyTaskEf MapStudyTaskToEf(StudyTask entity)
        {
            return new StudyTaskEf
            {
                Id = entity.Id,
                PlanCourseId = entity.PlanCourseId,
                CourseTopicId = entity.CourseTopicId,
                TaskName = entity.TaskName,
                TaskDescription = entity.TaskDescription,
                EstimatedHours = entity.EstimatedHours,
                DueDate = entity.DueDate,
                ScheduledDate = entity.ScheduledDate,
                Status = entity.Status,
                CompletionDate = entity.CompletionDate,
                PlanCourse = null, // Avoid circular reference
                CourseTopic = null, // Avoid circular reference
                Logs = entity.Logs?.Select(MapStudyLogToEf).ToList(),
                Resources = entity.Resources?.Select(MapTaskResourceToEf).ToList()
            };
        }

        private StudyLog MapStudyLog(StudyLogEf ef)
        {
            return new StudyLog
            {
                Id = ef.Id,
                TaskId = ef.TaskId,
                ActualTimeSpent = ef.ActualTimeSpent,
                LogDate = ef.LogDate
            };
        }

        private StudyLogEf MapStudyLogToEf(StudyLog entity)
        {
            return new StudyLogEf
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                ActualTimeSpent = entity.ActualTimeSpent,
                LogDate = entity.LogDate
            };
        }

        private TaskResource MapTaskResource(TaskResourceEf ef)
        {
            return new TaskResource
            {
                Id = ef.Id,
                TaskId = ef.TaskId,
                ResourceType = Enum.TryParse<ResourceType>(ef.ResourceType, out var type) ? type : ResourceType.Other,
                ResourceURL = ef.ResourceURL
            };
        }

        private TaskResourceEf MapTaskResourceToEf(TaskResource entity)
        {
            return new TaskResourceEf
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                ResourceType = entity.ResourceType.ToString(),
                ResourceURL = entity.ResourceURL
            };
        }
    }
}