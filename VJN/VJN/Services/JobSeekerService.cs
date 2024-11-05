using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using VJN.Models;
using VJN.ModelsDTO.CvDTOs;
using VJN.ModelsDTO.FavoriteListDTOs;
using VJN.ModelsDTO.ItemOfCvDTOs;
using VJN.ModelsDTO.JobSeekerDTOs;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;
using VJN.Repositories;

namespace VJN.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRespository  _jobSeekerRespository;
        private readonly IMapper _mapper;

        private const int PageSize = 6;

        public JobSeekerService(IJobSeekerRespository jobSeekerRespository, IMapper mapper)
        {
            _jobSeekerRespository = jobSeekerRespository;
            _mapper = mapper;
        }

        public async Task<bool> AddFavorite(FavoriteListCreateDTO model, int userid)
        {
            var fl = _mapper.Map<FavoriteList>(model);
            fl.EmployerId = userid;
            var check =  await _jobSeekerRespository.AddFavorite(fl);
            return check;
        }

        public async Task<bool> DeleteFavorite(int JobseekerID, int userid)
        {
            var check = await _jobSeekerRespository.DeleteFavorite(JobseekerID, userid);
            return check;
        }

        public async Task<PagedResult<JobSeekerDTO>> GetAllFavoriteList(FavoriteListSearch s, int userid)
        {
            var ids = await _jobSeekerRespository.GetAllFavoriteId(s, userid);

            var p = PaginationHelper.GetPaged<int>(ids, s.pageNumber.Value, PageSize);

            var user = await _jobSeekerRespository.GetAllFavorite(p.Items);

            var jobSeekerDTOsTask = user.Select(async x => new JobSeekerDTO()
            {
                UserId = x.UserId,
                AvatarURL = x.AvatarNavigation.Url,
                FullName = x.FullName,
                Age = x.Age,
                CurrentJob= x.CurrentJobNavigation.JobName,
                Address = x.Address,
                Gender = x.Gender,
                DescriptionFavorite = x.FavoriteListJobSeekers.Where(fl=>fl.EmployerId==userid).FirstOrDefault().Description,
                NumberApplied = x.ApplyJobs.Count(),
                NumberAppliedAccept = x.ApplyJobs.Where(aj=>aj.Status==3||aj.Status==4).Count(),
            });

            var jobSeekerDTOs = await Task.WhenAll(jobSeekerDTOsTask);

            var paged = new PagedResult<JobSeekerDTO>(jobSeekerDTOs, ids.Count(), s.pageNumber.Value, PageSize);
            return paged;
        }

        public async Task<JobSeekerDetailDTO> GetJobSeekerByIserID(int userID)
        {
            var user = await _jobSeekerRespository.GetJobSeekerByIserID(userID);
            var dto = _mapper.Map<JobSeekerDetailDTO>(user);


            var cvmodel = await _jobSeekerRespository.GetCVByUserId(userID);

            var cvdtos = new List<CvDTODetail>();
            foreach(var cv  in cvmodel)
            {
                var itcvdtos = new List<ItemOfcvDTOforView>();
                foreach (var itcv in cv.ItemOfCvs)
                {
                    var itcvdto = _mapper.Map<ItemOfcvDTOforView>(itcv);
                    itcvdtos.Add(itcvdto);
                }
                var cvdto = _mapper.Map<CvDTODetail>(cv);
                cvdto.ItemOfCvs = itcvdtos;
                cvdtos.Add(cvdto);
            }

            dto.CvDTOs = cvdtos;
            return dto;
        }
    }
}
