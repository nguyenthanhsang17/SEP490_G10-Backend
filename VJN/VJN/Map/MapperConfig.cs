using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using AutoMapper;
using VJN.ModelsDTO.BlogDTOs;
using VJN.ModelsDTO.MediaItemDTOs;

namespace VJN.Map
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //Mapper for user
            CreateMap<User, UserDTO>().ForMember(dest => dest.RoleName, opt=>opt.MapFrom(src=>src.Role.RoleName))
                                      .ForMember(dest=> dest.AvatarURL, opt=>opt.MapFrom(src=>src.AvatarNavigation.Url))
                                      .ForMember(dest=> dest.JobName, opt=>opt.MapFrom(src=>src.CurrentJobNavigation.JobName));
            CreateMap<UserCreateDTO, User>().ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 1))
                                            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 0))
                                            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => 4));
            CreateMap<UserUpdateDTO, User>();
            //Mapper for user
            //Mapper for Blog
            CreateMap<Blog, BlogDTO>();
            //Mapper for Blog

            //Maper for Media
            CreateMap<MediaItemDTO, MediaItem>();
            //Maper for Media
        }
    }
}
