using System;

namespace Ucm.Application.DTOs.StudyPlan
{
    public class StudyPlanDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int? WeeklyStudyGoalHours { get; set; }
        public int? CourseCount { get; set; }  // Số môn đăng ký
        public bool? Completed { get; set; } // Trạng thái hoàn thành
        // You can add more properties or nested DTOs if needed
    }
}