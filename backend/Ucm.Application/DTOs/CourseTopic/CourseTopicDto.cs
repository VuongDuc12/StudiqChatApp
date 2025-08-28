using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Application.DTOs.CourseTopic
{
    public class CreateCourseTopicDto
    {
        public int CourseId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCourseTopicDto
    {
        public string TopicName { get; set; }
        public string Description { get; set; }
    }

    public class CourseTopicDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
    }

}
