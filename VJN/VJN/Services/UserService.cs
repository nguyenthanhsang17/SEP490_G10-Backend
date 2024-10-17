using AutoMapper;
using VJN.ModelsDTO.UserDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> findById(int id)
        {
            var user = await _userRepository.findById(id);
            var userdto = _mapper.Map<UserDTO>(user);
            return userdto;
        }

        public async Task<IEnumerable<UserDTO>> getAllUser()
        {
            var users = await _userRepository.getAllUser();
            var userdto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userdto;
        }

        public async Task<UserDTO> Login(string Username, string Password)
        {
            var user = await _userRepository.Login(Username, Password);
            var UserDTO = _mapper.Map<UserDTO>(user);
            return UserDTO;
        }
    }
}
