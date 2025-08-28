using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class NotificationEntityEfMapper : IEntityEfMapper<Notification, NotificationEf>
    {
        public Notification ToEntity(NotificationEf ef)
        {
            if (ef == null) return null;

            return new Notification
            {
                Id = ef.Id,
                UserId = ef.UserId,
                Title = ef.Title,
                Message = ef.Message,
                CreatedAt = ef.CreatedAt,
                IsRead = ef.IsRead,
                Type = ef.Type,
                RelatedTaskId = ef.RelatedTaskId
            };
        }

        public NotificationEf ToEf(Notification entity)
        {
            if (entity == null) return null;

            return new NotificationEf
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Title = entity.Title,
                Message = entity.Message,
                CreatedAt = entity.CreatedAt,
                IsRead = entity.IsRead,
                Type = entity.Type,
                RelatedTaskId = entity.RelatedTaskId
            };
        }
    }
}