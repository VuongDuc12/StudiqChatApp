using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class TemplateTaskResourceEntityEfMapper : IEntityEfMapper<TemplateTaskResource, TemplateTaskResourceEf>
    {
        public TemplateTaskResource ToEntity(TemplateTaskResourceEf ef)
        {
            if (ef == null) return null;

            return new TemplateTaskResource
            {
                Id = ef.Id,
                TemplateTaskId = ef.TemplateTaskId,
                ResourceType = ef.ResourceType,
                ResourceURL = ef.ResourceURL
            };
        }

        public TemplateTaskResourceEf ToEf(TemplateTaskResource entity)
        {
            if (entity == null) return null;

            return new TemplateTaskResourceEf
            {
                Id = entity.Id,
                TemplateTaskId = entity.TemplateTaskId,
                ResourceType = entity.ResourceType,
                ResourceURL = entity.ResourceURL
            };
        }
    }
}