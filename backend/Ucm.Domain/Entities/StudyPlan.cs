using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Domain.Entities
{
    public class StudyPlan
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
        public ICollection<StudyPlanCourse> PlanCourses { get; set; }
    }
}
