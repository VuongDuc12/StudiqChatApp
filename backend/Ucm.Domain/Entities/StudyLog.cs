using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Domain.Entities
{
    public class StudyLog
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ActualTimeSpent { get; set; } // minutes
        public DateTime LogDate { get; set; }
    }
}
