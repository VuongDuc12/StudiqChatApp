using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using System.Linq;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class CourseEntityEfMapper : IEntityEfMapper<Course, CourseEf>
    {
        private readonly CourseTopicEntityEfMapper _topicMapper = new();

        public Course ToEntity(CourseEf ef)
        {
            if (ef == null) return null;

            return new Course
            {
                Id = ef.Id,
                CourseName = ef.CourseName,
                Credits = ef.Credits,
                Description = ef.Description,
                Topics = ef.Topics?.Select(_topicMapper.ToEntity).ToList()
            };
        }

        public CourseEf ToEf(Course entity)
        {
            if (entity == null) return null;

            return new CourseEf
            {
                Id = entity.Id,
                CourseName = entity.CourseName,
                Credits = entity.Credits,
                Description = entity.Description,
                Topics = entity.Topics?.Select(_topicMapper.ToEf).ToList()
            };
        }
    }
}
