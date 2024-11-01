using Microsoft.AspNetCore.Mvc.RazorPages;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;

namespace VJN.Services
{
    public interface IPostJobService
    {
        public Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob();
        public Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch);
        public Task<PostJobDetailDTO> getJostJobByID(int id, int? userid);
        public Task<PostJobDTOForList> GetPostJobById(int id);
        public Task<bool> ChangeStatusPostJob(int jobID, int status);
        public Task<int> CreatePostJob(PostJobCreateDTO postJob, int uid);
        public Task<PagedResult<JobSearchResultEmployer>> GetJobListByEmployerID(int employerID, PostJobSearchEmployer s);
        public Task<IEnumerable<PostJobDTOforReport>> GetAllPostJob(int status);
    }
}
