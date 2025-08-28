using System;
using System.Collections.Generic;

namespace Ucm.Application.DTOs.StudyPlan
{
    public class StudyPlanAdminSummaryDto
    {
        public int TotalUsers { get; set; }
        public int TotalPlans { get; set; }
        public int TotalActivePlans { get; set; }
        public int TotalCompletedPlans { get; set; }
        public int TotalPendingPlans { get; set; }
        public List<UserPlanSummaryDto> Users { get; set; } = new List<UserPlanSummaryDto>();
    }

    public class UserPlanSummaryDto
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
} 