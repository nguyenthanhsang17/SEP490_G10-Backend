using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ChatDTOs;

namespace VJN.Services
{
    public interface IChatService
    {
        public Task<IEnumerable<ChatListDTO>> GetChatUsers(int userid);
        public Task<IEnumerable<ChatDTO>> GetAllChat(int userId1, int userId2);

        public Task SendMessage(SendChat sendChat);
    }
}
