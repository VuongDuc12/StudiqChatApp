using System;
using System.Collections.Generic;

namespace Ucm.Application.DTOs.StudyPlan
{
    public class StudyPlanUserSummaryDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int TotalPlans { get; set; }
        public int ActivePlans { get; set; }
        public int CompletedPlans { get; set; }
        public int PendingPlans { get; set; }
        public List<StudyPlanSummaryDto> Plans { get; set; } = new List<StudyPlanSummaryDto>();
    }

    public class StudyPlanSummaryDto
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int? CourseCount { get; set; }
        public bool? Completed { get; set; }
        public string Status { get; set; } // "Active", "Completed", "Pending"
    }
} 