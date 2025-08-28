using System;
using System.Collections.Generic;

namespace Ucm.Domain.Entities
{
    public class StudyPlanCourse
    {
        public int Id { get; set; }
        public int StudyPlanId { get; set; }
        public StudyPlan StudyPlan { get; set; }
        public int CourseId { get; set; }
        public Guid UserId { get; set; }
        
        // Navigation properties
        public Course Course { get; set; }
        public ICollection<StudyTask> Tasks { get; set; }
    }
}