using VJN.Models;
using VJN.ModelsDTO.ChatDTOs;

namespace VJN.Repositories
{
    public interface IChatRepository
    {
        public Task<IEnumerable<User>> GetChatUsers(int userid);
        public Task<IEnumerable<Chat>> GetAllChat(int userId1, int userId2);
        public Task SendMessage(Chat sendChat);
    }
}
