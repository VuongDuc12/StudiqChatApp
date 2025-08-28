using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class CourseTopicEf
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public CourseEf Course { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
    }

}
