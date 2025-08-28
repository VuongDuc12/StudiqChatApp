using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;

namespace Ucm.Infrastructure.Data.Models
{
    public class AppUserEF : IdentityUser<Guid>
    {
       
        public string FullName { get; set; }
       
        public ICollection<StudyPlanEf> StudyPlans { get; set; }
        public ICollection<NotificationEf> Notifications { get; set; }
    }
}

