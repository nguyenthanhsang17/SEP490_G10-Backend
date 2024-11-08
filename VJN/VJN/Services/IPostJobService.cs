using Microsoft.AspNetCore.Mvc.RazorPages;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.ReportDTO;
using VJN.Paging;

namespace VJN.Services
{
    public interface IPostJobService
    {
        public Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob();
        public Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch, int? userid);
        public Task<PostJobDetailDTO> getJostJobByID(int id, int? userid);
        public Task<PostJobDTOForList> GetPostJobById(int id);
        public Task<bool> ChangeStatusPostJob(int jobID, int status);
        public Task<int> CreatePostJob(PostJobCreateDTO postJob, int uid);
        public Task<PagedResult<JobSearchResultEmployer>> GetJobListByEmployerID(int employerID, PostJobSearchEmployer s);
<<<<<<< HEAD
        public Task<IEnumerable<PostJobDTOforReport>> GetAllPostJobByStatus(int status);
        public  Task<PostJobDTOReport> GetPostByIDForStaff(int id);
=======
        public Task<IEnumerable<PostJobDTOforReport>> GetAllPostJob(int status);

        public Task<bool> AddWishJob(int jobid, int userid);
        public Task<bool> DeleteWishJob(int jobid, int userid);
        public Task<PagedResult<JobSearchResult>> getJobWishList(PostJobSearchWishList s, int userid);
        public Task<int> ReportJob(ReportCreateDTO report, int userid);
>>>>>>> 79fa441ff4e50fcad1fc1cf0fb84c14fb9c45118
    }
}
