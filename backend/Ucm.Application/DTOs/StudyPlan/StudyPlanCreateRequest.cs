using System;

namespace Ucm.Application.DTOs.StudyPlan
{
    public class StudyPlanCreateRequest
    {
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int? WeeklyStudyGoalHours { get; set; }
    }
}