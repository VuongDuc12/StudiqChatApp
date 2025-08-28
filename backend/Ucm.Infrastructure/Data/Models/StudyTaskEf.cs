using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class StudyTaskEf
    {
        public int Id { get; set; }
        public int PlanCourseId { get; set; }
        public StudyPlanCourseEf PlanCourse { get; set; }
        public int? CourseTopicId { get; set; }
        public CourseTopicEf? CourseTopic { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public string Status { get; set; } = "ToDo";
        public DateTime? CompletionDate { get; set; }
        public ICollection<StudyLogEf> Logs { get; set; }
        public ICollection<TaskResourceEf> Resources { get; set; }
    }
}
