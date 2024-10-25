using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using AutoMapper;
using VJN.ModelsDTO.BlogDTOs;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.CvDTOs;
using VJN.ModelsDTO.ItemOfCvDTOs;
using VJN.ModelsDTO.SlotDTOs;
using VJN.ModelsDTO.ApplyJobDTOs;

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

            CreateMap<User, UserDTOdetail>();


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
            CreateMap<PostJob, PostJobDetailDTO>().ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                                            .ForMember(dest => dest.JobCategoryName, opt => opt.MapFrom(src => src.JobCategory.JobCategoryName))
                                            .ForMember(dest => dest.SalaryTypeName, opt => opt.MapFrom(src => src.SalaryTypes.TypeName));

            CreateMap<PostJobCreateDTO, PostJob>().ForMember(dest=>dest.CreateDate, opt=>opt.MapFrom(src=> DateTime.Now));

            CreateMap<Cv, CvDTODetail>();
            CreateMap<ItemOfCv, ItemOfcvDTOforView>();


            CreateMap<Slot, SlotDTO>();
            CreateMap<JobSchedule, JobScheduleDTO>();
            CreateMap<WorkingHour, WorkingHourDTO>();


            CreateMap<ApplyJobCreateDTO, ApplyJob>().ForMember(dest=>dest.ApplyDate, otp=>otp.MapFrom(src=>DateTime.Now))
                                                    .ForMember(dest=>dest.Status, otp=>otp.MapFrom(src=>0));
        }
    }
}
