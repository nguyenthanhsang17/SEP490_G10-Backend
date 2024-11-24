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
            var query = await _context.Chats
                .Where(c => c.SendFromId == userid || c.SendToId == userid)
                .GroupBy(c => c.SendFromId == userid ? c.SendToId : c.SendFromId)
                .Select(g => new
                {
                    UserId = g.Key,
                    LastMessageTime = g.Max(c => c.SendTime)
                })
                .Join(_context.Users,
                    chatUser => chatUser.UserId,
                    user => user.UserId,
                    (chatUser, user) => new { User = user, LastMessageTime = chatUser.LastMessageTime })
                .OrderByDescending(u => u.LastMessageTime)
                .Select(u => u.User)
                .ToListAsync();

            return query;
        }

        public async Task SendMessage(Chat sendChat)
        {
            _context.Chats.Add(sendChat);
            await _context.SaveChangesAsync();
        }
    }
}
