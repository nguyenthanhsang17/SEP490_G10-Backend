using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using AutoMapper;
using VJN.ModelsDTO.BlogDTOs;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.CvDTOs;
using VJN.ModelsDTO.ItemOfCvDTOs;

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
            CreateMap<UserDTO, UserDTOforList>();
            CreateMap<User, UserDTOdetail>().ForMember(dest => dest.AvatarURL, opt => opt.MapFrom(src => src.AvatarNavigation.Url));
            //Mapper for user
            //Mapper for Blog
            CreateMap<Blog, BlogDTO>();
            //Mapper for Blog

            //Maper for Media
            CreateMap<MediaItemDTO, MediaItem>();
            //Maper for Media

            CreateMap<PostJob, PostJobDTOForHomepage>().ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                                            .ForMember(dest => dest.JobCategoryName, opt => opt.MapFrom(src => src.JobCategory.JobCategoryName))
                                            .ForMember(dest => dest.SalaryTypeName, opt => opt.MapFrom(src => src.SalaryTypes.TypeName));
            CreateMap<PostJob, PostJobDTOForList>();
            CreateMap<Cv, CvDTODetail>();
            CreateMap<ItemOfCv, ItemOfcvDTOforView>();
        }
    }
}
