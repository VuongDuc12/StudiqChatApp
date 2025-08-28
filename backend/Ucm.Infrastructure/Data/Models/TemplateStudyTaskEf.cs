using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class TemplateStudyTaskEf
    {
        public int Id { get; set; }
        public int TemplatePlanId { get; set; }
        public TemplateStudyPlanEf TemplatePlan { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int DayOffset { get; set; }
        public ICollection<TemplateTaskResourceEf> Resources { get; set; }
    }

}
