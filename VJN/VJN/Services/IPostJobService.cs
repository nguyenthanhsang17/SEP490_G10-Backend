using Microsoft.AspNetCore.Mvc.RazorPages;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;

namespace VJN.Services
{
    public interface IPostJobService
    {
        public Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob();
        public Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch, int pageNumber);
        public Task<PostJob> getJostJobByID(int id);

        public Task<PostJobDTOForList> GetPostJobById(int id);


    }
}
