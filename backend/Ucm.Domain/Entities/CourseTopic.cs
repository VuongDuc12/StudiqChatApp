using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Domain.Entities
{
    public class CourseTopic
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
    }
}
