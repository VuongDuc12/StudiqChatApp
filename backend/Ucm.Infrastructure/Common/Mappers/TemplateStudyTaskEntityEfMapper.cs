using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using System.Linq;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class TemplateStudyTaskEntityEfMapper : IEntityEfMapper<TemplateStudyTask, TemplateStudyTaskEf>
    {
        private readonly TemplateTaskResourceEntityEfMapper _resourceMapper = new();

        public TemplateStudyTask ToEntity(TemplateStudyTaskEf ef)
        {
            if (ef == null) return null;

            return new TemplateStudyTask
            {
                Id = ef.Id,
                TemplatePlanId = ef.TemplatePlanId,
                TaskName = ef.TaskName,
                TaskDescription = ef.TaskDescription,
                EstimatedHours = ef.EstimatedHours,
                DayOffset = ef.DayOffset,
                Resources = ef.Resources?.Select(_resourceMapper.ToEntity).ToList()
            };
        }

        public TemplateStudyTaskEf ToEf(TemplateStudyTask entity)
        {
            if (entity == null) return null;

            return new TemplateStudyTaskEf
            {
                Id = entity.Id,
                TemplatePlanId = entity.TemplatePlanId,
                TaskName = entity.TaskName,
                TaskDescription = entity.TaskDescription,
                EstimatedHours = entity.EstimatedHours,
                DayOffset = entity.DayOffset,
                Resources = entity.Resources?.Select(_resourceMapper.ToEf).ToList()
            };
        }
    }
}