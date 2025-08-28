using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Enums;

namespace Ucm.Domain.Entities
{
    public class TaskResource
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceURL { get; set; }
    }

}
