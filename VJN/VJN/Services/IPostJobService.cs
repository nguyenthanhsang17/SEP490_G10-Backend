using Microsoft.AspNetCore.Mvc.RazorPages;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;

namespace VJN.Services
{
    public interface IPostJobService
    {
        public Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob();

<<<<<<< HEAD
        public Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch, int pageNumber);
=======
        public Task<PostJobDTOForList> GetPostJobById(int id);
>>>>>>> 6ffb2ae (Inter 1)

    }
}
