using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using AutoMapper;
using VJN.ModelsDTO.BlogDTOs;

namespace VJN.Map
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //Mapper for user
            CreateMap<User, UserDTO>().ForMember(dest => dest.RoleName, opt=>opt.MapFrom(src=>src.Role.RoleName));
            CreateMap<UserCreateDTO, User>().ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 1))
                                            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
                                            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => "https://i0.wp.com/sbcf.fr/wp-content/uploads/2018/03/sbcf-default-avatar.png?ssl=1"));
            //Mapper for user
            //Mapper for Blog
            CreateMap<Blog, BlogDTO>();
            //Mapper for Blog
        }
    }
}
