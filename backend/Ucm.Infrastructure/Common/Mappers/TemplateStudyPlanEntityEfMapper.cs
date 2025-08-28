using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using System.Linq;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class TemplateStudyPlanEntityEfMapper : IEntityEfMapper<TemplateStudyPlan, TemplateStudyPlanEf>
    {
        private readonly TemplateStudyTaskEntityEfMapper _taskMapper = new();

        public TemplateStudyPlan ToEntity(TemplateStudyPlanEf ef)
        {
            if (ef == null) return null;

            return new TemplateStudyPlan
            {
                Id = ef.Id,
                CourseId = ef.CourseId,
                TemplateName = ef.TemplateName,
                Description = ef.Description,
                Tasks = ef.Tasks?.Select(_taskMapper.ToEntity).ToList()
            };
        }

        public TemplateStudyPlanEf ToEf(TemplateStudyPlan entity)
        {
            if (entity == null) return null;

            return new TemplateStudyPlanEf
            {
                Id = entity.Id,
                CourseId = entity.CourseId,
                TemplateName = entity.TemplateName,
                Description = entity.Description,
                Tasks = entity.Tasks?.Select(_taskMapper.ToEf).ToList()
            };
        }
    }
}