using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucm.Domain.Enums;

namespace Ucm.Domain.Entities
{
    public class StudyTask
    {
        public int Id { get; set; }
        public int PlanCourseId { get; set; }
        public StudyPlanCourse PlanCourse { get; set; }
        public int? CourseTopicId { get; set; }
        public CourseTopic CourseTopic { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public string Status { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ICollection<StudyLog> Logs { get; set; }
        public ICollection<TaskResource> Resources { get; set; }
    }
}
