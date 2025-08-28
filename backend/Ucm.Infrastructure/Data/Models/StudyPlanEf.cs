using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class StudyPlanEf
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public AppUserEF User { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int? WeeklyStudyGoalHours { get; set; }
        public int? CourseCount { get; set; }  // Số môn đăng ký
        public bool? Completed { get; set; } // Trạng thái hoàn thành
        public ICollection<StudyPlanCourseEf> PlanCourses { get; set; }
    }


}
