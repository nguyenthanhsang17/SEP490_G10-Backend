using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using AutoMapper;

namespace VJN.Map
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.RoleName, opt=>opt.MapFrom(src=>src.Role.RoleName));
        }
    }
}
