using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.ChatDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class ChatService : IChatService
    {

        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatDTO>> GetAllChat(int userId1, int userId2)
        {
            var chats = await _chatRepository.GetAllChat(userId1, userId2);
            var chatdto = _mapper.Map<IEnumerable<ChatDTO>>(chats);
            return chatdto;
        }

        public async Task<IEnumerable<ChatListDTO>> GetChatUsers(int userid)
        {
            var result = await _chatRepository.GetChatUsers(userid);

            var resultdto = _mapper.Map<IEnumerable<ChatListDTO>>(result);

            return resultdto;
        }

        public async Task SendMessage(SendChat sendChat)
        {
            var chat = _mapper.Map<Chat>(sendChat);
            await _chatRepository.SendMessage(chat);
        }
    }
}
