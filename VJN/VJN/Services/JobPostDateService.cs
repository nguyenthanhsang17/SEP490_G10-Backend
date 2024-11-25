using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.JobPostDateDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class JobPostDateService : IJobPostDateService
    {
        public readonly IJobPostDateRepository _jobPostDateRepository;
        private readonly IMapper _mapper;

        public JobPostDateService(IJobPostDateRepository jobPostDateRepository, IMapper mapper)
        {
            _jobPostDateRepository = jobPostDateRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs)
        {
            var c = await _jobPostDateRepository.CreateJobPostDate(jobPostDateCreateDTOs);
            return c;
        }

        public async Task<bool> DeleteAllJobPostDate(int postid)
        {
            var c=await _jobPostDateRepository.DeleteAllJobPostByPOstID(postid);
            return c;
        }

        public async Task<IEnumerable<JobPostDateDTO>> GetPostJobByPostID(int postid)
        {
            var pj = await _jobPostDateRepository.GetPostJobByPostID(postid);
            var pjdto = _mapper.Map<IEnumerable<JobPostDateDTO>>(pj);
            var pjdto2 = pjdto.OrderBy(pj=>pj.EventDate).ThenBy(pj=>pj.StartTime);
            return pjdto2;   
        }

        public async Task<bool> UpdateJobPostDate(int postid, IEnumerable<JobPostDateForUpdateDTO> jobPostDates)
        {
            var c1 = await _jobPostDateRepository.DeleteAllJobPostByPOstID(postid);
            Console.WriteLine("c1: "+c1);
            if (c1)
            {
                var c2 = await _jobPostDateRepository.UpdateAllDate(postid, jobPostDates);
                Console.WriteLine("c2: "+c2);
                return c2;
            }
            return false;
        }
    }
}
