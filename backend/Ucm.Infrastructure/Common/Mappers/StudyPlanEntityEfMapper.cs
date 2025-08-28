using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using System.Linq;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class StudyPlanEntityEfMapper : IEntityEfMapper<StudyPlan, StudyPlanEf>
    {
        private readonly StudyPlanCourseEntityEfMapper _planCourseMapper = new();

        public StudyPlan ToEntity(StudyPlanEf ef)
        {
            if (ef == null) return null;

            return new StudyPlan
            {
                Id = ef.Id,
                UserId = ef.UserId,
                PlanName = ef.PlanName,
                StartDate = ef.StartDate,
                EndDate = ef.EndDate,
                Semester = ef.Semester,
                AcademicYear = ef.AcademicYear,
                WeeklyStudyGoalHours = ef.WeeklyStudyGoalHours,
                CourseCount = ef.CourseCount,
                Completed = ef.Completed,
                PlanCourses = ef.PlanCourses?.Select(_planCourseMapper.ToEntity).ToList()
            };
        }

        public StudyPlanEf ToEf(StudyPlan entity)
        {
            if (entity == null) return null;

            return new StudyPlanEf
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PlanName = entity.PlanName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Semester = entity.Semester,
                AcademicYear = entity.AcademicYear,
                WeeklyStudyGoalHours = entity.WeeklyStudyGoalHours,
                CourseCount = entity.CourseCount,
                Completed = entity.Completed,
                PlanCourses = entity.PlanCourses?.Select(_planCourseMapper.ToEf).ToList()
            };
        }
    }
}