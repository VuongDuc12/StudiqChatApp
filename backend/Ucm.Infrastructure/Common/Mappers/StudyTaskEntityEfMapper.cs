using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using System.Linq;
using Ucm.Domain.Enums;
using System;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class StudyTaskEntityEfMapper : IEntityEfMapper<StudyTask, StudyTaskEf>
    {
        public StudyTask ToEntity(StudyTaskEf ef)
        {
            if (ef == null) return null;

            Console.WriteLine($"Mapping StudyTaskEf to StudyTask - Id: {ef.Id}");
            Console.WriteLine($"EF PlanCourse: {ef.PlanCourse != null}");
            if (ef.PlanCourse != null)
            {
                Console.WriteLine($"EF StudyPlan: {ef.PlanCourse.StudyPlan != null}, Course: {ef.PlanCourse.Course != null}");
            }

            var result = new StudyTask
            {
                Id = ef.Id,
                PlanCourseId = ef.PlanCourseId,
                PlanCourse = ef.PlanCourse != null ? MapPlanCourse(ef.PlanCourse) : null,
                CourseTopicId = ef.CourseTopicId,
                CourseTopic = ef.CourseTopic != null ? MapCourseTopic(ef.CourseTopic) : null,
                TaskName = ef.TaskName,
                TaskDescription = ef.TaskDescription,
                EstimatedHours = ef.EstimatedHours,
                DueDate = ef.DueDate,
                ScheduledDate = ef.ScheduledDate,
                Status = ef.Status,
                CompletionDate = ef.CompletionDate,
                Logs = ef.Logs?.Select(MapStudyLog).ToList(),
                Resources = ef.Resources?.Select(MapTaskResource).ToList()
            };

            Console.WriteLine($"Mapped to StudyTask - Id: {result.Id}");
            Console.WriteLine($"Domain PlanCourse: {result.PlanCourse != null}");
            if (result.PlanCourse != null)
            {
                Console.WriteLine($"Domain StudyPlan: {result.PlanCourse.StudyPlan != null}, Course: {result.PlanCourse.Course != null}");
            }

            return result;
        }

        public StudyTaskEf ToEf(StudyTask entity)
        {
            if (entity == null) return null;

            return new StudyTaskEf
            {
                Id = entity.Id,
                PlanCourseId = entity.PlanCourseId,
                PlanCourse = entity.PlanCourse != null ? MapPlanCourseToEf(entity.PlanCourse) : null,
                CourseTopicId = entity.CourseTopicId,
                CourseTopic = entity.CourseTopic != null ? MapCourseTopicToEf(entity.CourseTopic) : null,
                TaskName = entity.TaskName,
                TaskDescription = entity.TaskDescription,
                EstimatedHours = entity.EstimatedHours,
                DueDate = entity.DueDate,
                ScheduledDate = entity.ScheduledDate,
                Status = entity.Status,
                CompletionDate = entity.CompletionDate,
                Logs = entity.Logs?.Select(MapStudyLogToEf).ToList(),
                Resources = entity.Resources?.Select(MapTaskResourceToEf).ToList()
            };
        }

        // Helper methods to avoid circular dependency
        private StudyPlanCourse MapPlanCourse(StudyPlanCourseEf ef)
        {
            if (ef == null) return null;

            Console.WriteLine($"Mapping StudyPlanCourseEf to StudyPlanCourse - Id: {ef.Id}");
            Console.WriteLine($"EF StudyPlan: {ef.StudyPlan != null}, Course: {ef.Course != null}");

            var result = new StudyPlanCourse
            {
                Id = ef.Id,
                StudyPlanId = ef.StudyPlanId,
                StudyPlan = ef.StudyPlan != null ? MapStudyPlan(ef.StudyPlan) : null,
                CourseId = ef.CourseId,
                Course = ef.Course != null ? MapCourse(ef.Course) : null,
                UserId = ef.UserId,
                Tasks = null // Avoid circular reference
            };

            Console.WriteLine($"Mapped to StudyPlanCourse - Id: {result.Id}");
            Console.WriteLine($"Domain StudyPlan: {result.StudyPlan != null}, Course: {result.Course != null}");

            return result;
        }

        private StudyPlanCourseEf MapPlanCourseToEf(StudyPlanCourse entity)
        {
            return new StudyPlanCourseEf
            {
                Id = entity.Id,
                StudyPlanId = entity.StudyPlanId,
                StudyPlan = entity.StudyPlan != null ? MapStudyPlanToEf(entity.StudyPlan) : null,
                CourseId = entity.CourseId,
                Course = entity.Course != null ? MapCourseToEf(entity.Course) : null,
                UserId = entity.UserId,
                Tasks = null // Avoid circular reference
            };
        }

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

        private CourseTopic MapCourseTopic(CourseTopicEf ef)
        {
            return new CourseTopic
            {
                Id = ef.Id,
                TopicName = ef.TopicName,
                Description = ef.Description,
                CourseId = ef.CourseId
            };
        }

        private CourseTopicEf MapCourseTopicToEf(CourseTopic entity)
        {
            return new CourseTopicEf
            {
                Id = entity.Id,
                TopicName = entity.TopicName,
                Description = entity.Description,
                CourseId = entity.CourseId
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