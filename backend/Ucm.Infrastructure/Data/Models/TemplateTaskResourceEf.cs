using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class TemplateTaskResourceEf
    {
        public int Id { get; set; }
        public int TemplateTaskId { get; set; }
        public TemplateStudyTaskEf TemplateTask { get; set; }
        public string ResourceType { get; set; }
        public string ResourceURL { get; set; }
    }
}
