using HotelApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Repositories
{
    public class CourseTopicRepository : RepositoryBase<CourseTopic, CourseTopicEf>, ICourseTopicRepository
    {
        public CourseTopicRepository(AppDbContext context, IEntityEfMapper<CourseTopic, CourseTopicEf> mapper)
            : base(context, mapper) { }

        protected override IQueryable<CourseTopicEf> ApplySearchFilter(IQueryable<CourseTopicEf> query, string searchTerm)
        {
            return query.Where(x => x.TopicName.Contains(searchTerm));
        }

    }
}
