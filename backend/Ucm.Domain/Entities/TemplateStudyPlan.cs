using System;
using System.Collections.Generic;

namespace Ucm.Domain.Entities
{
    public class TemplateStudyPlan
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public ICollection<TemplateStudyTask> Tasks { get; set; }
    }
}