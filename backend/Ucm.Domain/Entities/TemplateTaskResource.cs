using System;

namespace Ucm.Domain.Entities
{
    public class TemplateTaskResource
    {
        public int Id { get; set; }
        public int TemplateTaskId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceURL { get; set; }
    }
}