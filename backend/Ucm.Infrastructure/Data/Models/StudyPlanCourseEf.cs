using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class StudyPlanCourseEf
    {
        public int Id { get; set; }
        public int StudyPlanId { get; set; }
        public StudyPlanEf StudyPlan { get; set; }
        public int CourseId { get; set; }
        public CourseEf Course { get; set; }
        public Guid UserId { get; set; }
        public AppUserEF User { get; set; }
        public ICollection<StudyTaskEf> Tasks { get; set; }
    }
}
