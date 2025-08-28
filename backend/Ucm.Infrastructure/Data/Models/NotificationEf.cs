using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Data.Models
{
    public class NotificationEf
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public AppUserEF User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public string Type { get; set; } = "System";
        public int? RelatedTaskId { get; set; }
        public StudyTaskEf? RelatedTask { get; set; }
    }
}
