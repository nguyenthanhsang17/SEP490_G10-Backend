using VJN.ModelsDTO.PostJobDTOs;

namespace VJN.Services
{
    public interface IPostJobService
    {
        public Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob();

        public Task<PostJobDTOForList> GetPostJobById(int id);

    }
}
