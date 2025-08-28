using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Domain.Entities
{

    public class AppUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public ICollection<UserEnrollment> Enrollments { get; set; }
        public ICollection<StudyPlan> StudyPlans { get; set; }
    }


}
