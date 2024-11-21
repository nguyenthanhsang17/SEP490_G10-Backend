using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class ChatRepository : IChatRepository
    {
        public readonly VJNDBContext _context;

        public ChatRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetAllChat(int userId1, int userId2)
        {
            var chats = await _context.Chats
            .Where(c =>
                (c.SendFromId == userId1 && c.SendToId == userId2) ||
                (c.SendFromId == userId2 && c.SendToId == userId1))
            .OrderBy(c => c.SendTime)
            .ToListAsync();

            return chats;
        }

        public async Task<IEnumerable<User>> GetChatUsers(int userid)
        {
            var users = await _context.Chats
                    .Where(c => c.SendFromId == userid || c.SendToId == userid)
                    .GroupBy(c => c.SendFromId == userid ? c.SendToId : c.SendFromId)
                    .Select(g => g.Key)
                    .Distinct()
                    .Join(_context.Users,
                            chatUserId => chatUserId,
                            user => user.UserId,
                            (chatUserId, user) => user)
                    .ToListAsync();
            return users;
        }

        public async Task SendMessage(Chat sendChat)
        {
            _context.Chats.Add(sendChat);
            await _context.SaveChangesAsync();
        }
    }
}
