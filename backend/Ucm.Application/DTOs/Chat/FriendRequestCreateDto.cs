namespace Ucm.Application.DTOs.Chat
{
    public class FriendRequestCreateDto
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }
}