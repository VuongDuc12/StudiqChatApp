using System;
using System.Collections.Generic;

namespace Ucm.Domain.Entities
{
    public class TemplateStudyTask
    {
        public int Id { get; set; }
        public int TemplatePlanId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int DayOffset { get; set; }
        public ICollection<TemplateTaskResource> Resources { get; set; }
    }
}