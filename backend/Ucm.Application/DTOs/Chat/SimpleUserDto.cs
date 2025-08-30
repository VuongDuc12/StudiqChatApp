using System;

namespace Ucm.Application.DTOs.Chat
{
    public class SimpleUserDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
