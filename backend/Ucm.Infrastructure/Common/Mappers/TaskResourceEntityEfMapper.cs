using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;
using Ucm.Domain.Enums;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class TaskResourceEntityEfMapper : IEntityEfMapper<TaskResource, TaskResourceEf>
    {
        public TaskResource ToEntity(TaskResourceEf ef)
        {
            if (ef == null) return null;

            return new TaskResource
            {
                Id = ef.Id,
                TaskId = ef.TaskId,
                ResourceType = Enum.TryParse<ResourceType>(ef.ResourceType, out var type) ? type : ResourceType.Other,
                ResourceURL = ef.ResourceURL
            };
        }

        public TaskResourceEf ToEf(TaskResource entity)
        {
            if (entity == null) return null;

            return new TaskResourceEf
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                ResourceType = entity.ResourceType.ToString(),
                ResourceURL = entity.ResourceURL
            };
        }
    }
}