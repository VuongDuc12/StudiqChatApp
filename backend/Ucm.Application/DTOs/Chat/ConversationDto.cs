using System;
using System.Collections.Generic;

namespace Ucm.Application.DTOs.Chat
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public SimpleUserDto? Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public MessageDto? LastMessage { get; set; }
        public int UnreadCount { get; set; }
        public List<SimpleUserDto> Members { get; set; } = new();
    }
}
