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
using VJN.ModelsDTO.ReportDTO;
using System.Drawing;
using VJN.ModelsDTO.RegisterEmployer;
using VJN.ModelsDTO.WishJob;
using VJN.ModelsDTO.FavoriteListDTOs;
using VJN.ModelsDTO.JobSeekerDTOs;
using VJN.ModelsDTO.JobPostDateDTOs;
using VJN.ModelsDTO.EmployerDTOs;
using VJN.ModelsDTO.ServicePriceLogDTOs;
using VJN.ModelsDTO.ServicePriceListDTOs;
using VJN.ModelsDTO.ChatDTOs;

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



            CreateMap<UserDTO, UserDTOforList>().ForMember(dest => dest.Apply_id, opt => opt.MapFrom(src => 0)); ;

            CreateMap<User, UserDTOdetail>();

            CreateMap<User, ChatListDTO>();


            CreateMap<User, UserDTOdetail>().ForMember(dest => dest.AvatarURL, opt => opt.MapFrom(src => src.AvatarNavigation.Url));

            CreateMap<User, EmployerDTO>().ForMember(dest => dest.PostJobAuthors, opt => opt.Ignore())
                                          .ForMember(dest=>dest.avatarURL, opt=>opt.MapFrom(src=>src.AvatarNavigation.Url)); 

            //Mapper for user
            //Mapper for Blog
            CreateMap<Blog, BlogDTO>().ForMember(dest=> dest.Thumbnail, opt => opt.MapFrom(src => src.ThumbnailNavigation.Url))
                                      .ForMember(dest=> dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName));
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
            CreateMap<PostJob, PostJobDTOforReport>().ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                                            .ForMember(dest => dest.JobCategoryName, opt => opt.MapFrom(src => src.JobCategory.JobCategoryName))
                                            .ForMember(dest => dest.SalaryTypeName, opt => opt.MapFrom(src => src.SalaryTypes.TypeName));
            CreateMap<Report, ReportDTO>().ForMember(dest => dest.jobseekerName, opt => opt.MapFrom(src => src.JobSeeker.FullName));
            CreateMap<PostJobCreateDTO, PostJob>().ForMember(dest=>dest.CreateDate, opt=>opt.MapFrom(src=> DateTime.Now)).ForMember(dest => dest.JobPostDates, opt => opt.Ignore()); ;
            CreateMap<Cv, CvDTODetail>();
            CreateMap<ItemOfCv, ItemOfcvDTOforView>();


            CreateMap<Slot, SlotDTO>();
            CreateMap<JobSchedule, JobScheduleDTO>();
            CreateMap<WorkingHour, WorkingHourDTO>();


            CreateMap<ApplyJobCreateDTO, ApplyJob>().ForMember(dest=>dest.ApplyDate, otp=>otp.MapFrom(src=>DateTime.Now))
                                                    .ForMember(dest=>dest.Status, otp=>otp.MapFrom(src=>0));

            CreateMap<FavoriteListCreateDTO, FavoriteList>();

            CreateMap<User, JobSeekerDetailDTO>().ForMember(dest => dest.AvatarURL, otp => otp.MapFrom(src => src.AvatarNavigation.Url))
                                                 .ForMember(dest => dest.CurrentJob, otp => otp.MapFrom(src => src.CurrentJobNavigation.JobName))
                                                 .ForMember(dest => dest.NumberAppiled, otp => otp.MapFrom(src => src.ApplyJobs.Count))
                                                 .ForMember(dest => dest.NumberAppiledAccept, otp => otp.MapFrom(src => src.ApplyJobs != null ? src.ApplyJobs.Count(aj => aj.Status == 3 || aj.Status == 4) : 0));

            CreateMap<User, JobSeekerForListDTO>().ForMember(dest => dest.AvatarURL, otp => otp.MapFrom(src => src.AvatarNavigation.Url))
                                                  .ForMember(dest => dest.CurrentJob, otp => otp.MapFrom(src => src.CurrentJobNavigation.JobName))
                                                  .ForMember(dest => dest.NumberApplied, otp => otp.MapFrom(src => src.ApplyJobs.Count))
                                                  .ForMember(dest => dest.NumberAppliedAccept, otp => otp.MapFrom(src => src.ApplyJobs != null ? src.ApplyJobs.Count(aj => aj.Status == 3 || aj.Status == 4) : 0));
            
            CreateMap<JobPostDate, JobPostDateDTO>();
            CreateMap<PostJobDetailUpdate, PostJob>();

            CreateMap<ServicePriceLog, PaymentHistory>();
            CreateMap<ServicePriceList, ServicePriceListDTO>();

            CreateMap<Chat, ChatDTO>();
            CreateMap<SendChat, Chat>().ForMember(dest => dest.SendTime, otp => otp.MapFrom(src => DateTime.Now));
            CreateMap<ServicePriceLogForCreateDTO, ServicePriceLog>().ForMember(dest => dest.RegisterDate, otp => otp.MapFrom(src => DateTime.Now));
        }
    }
}
