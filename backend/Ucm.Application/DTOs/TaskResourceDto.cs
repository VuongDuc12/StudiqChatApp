using Ucm.Domain.Enums;

namespace Ucm.Application.Dtos
{
    public class TaskResourceDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceURL { get; set; }
    }
}