using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class CourseTopicEntityEfMapper : IEntityEfMapper<CourseTopic, CourseTopicEf>
    {
        public CourseTopic ToEntity(CourseTopicEf ef)
        {
            if (ef == null) return null;

            return new CourseTopic
            {
                Id = ef.Id,
                CourseId = ef.CourseId,
                TopicName = ef.TopicName,
                Description = ef.Description
            };
        }

        public CourseTopicEf ToEf(CourseTopic entity)
        {
            if (entity == null) return null;

            return new CourseTopicEf    
            {
                Id = entity.Id,
                CourseId = entity.CourseId,
                TopicName = entity.TopicName,
                Description = entity.Description
            };
        }
    }
}