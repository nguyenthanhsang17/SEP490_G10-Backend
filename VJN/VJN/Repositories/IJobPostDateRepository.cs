using VJN.Models;
using VJN.ModelsDTO.JobPostDateDTOs;

namespace VJN.Repositories
{
    public interface IJobPostDateRepository
    {
        public Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs);
        public Task<IEnumerable<JobPostDate>> GetPostJobByPostID(int postid);

        public Task<bool> DeleteAllJobPostByPOstID(int postid);

        public Task<bool> UpdateAllDate(int postid, IEnumerable<JobPostDateForUpdateDTO> jobPostDates);
        }
}
