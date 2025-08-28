using Ucm.Domain.Entities;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Common.Mappers
{
    public class StudyLogEntityEfMapper : IEntityEfMapper<StudyLog, StudyLogEf>
    {
        public StudyLog ToEntity(StudyLogEf ef)
        {
            if (ef == null) return null;

            return new StudyLog
            {
                Id = ef.Id,
                TaskId = ef.TaskId,
                ActualTimeSpent = ef.ActualTimeSpent,
                LogDate = ef.LogDate
            };
        }

        public StudyLogEf ToEf(StudyLog entity)
        {
            if (entity == null) return null;

            return new StudyLogEf
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                ActualTimeSpent = entity.ActualTimeSpent,
                LogDate = entity.LogDate
            };
        }
    }
}