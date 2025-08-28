using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class TemplateStudyPlanEf
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public CourseEf Course { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public ICollection<TemplateStudyTaskEf> Tasks { get; set; }
    }
}
