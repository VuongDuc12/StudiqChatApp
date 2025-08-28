using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class TaskResourceEf
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public StudyTaskEf Task { get; set; }
        public string ResourceType { get; set; }
        public string ResourceURL { get; set; }
    }

}
