using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class PostJobService : IPostJobService
    {
        private readonly IPostJobRepository _postJobRepository;
        private readonly IMapper _mapper;
        public PostJobService(IPostJobRepository postJobRepository, IMapper mapper) {
            _postJobRepository = postJobRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob()
        {
            var p = await _postJobRepository.GetPorpularJob();
            var pdto = _mapper.Map<IEnumerable<PostJobDTOForHomepage>>(p);
            foreach (var dto in pdto)
            {
                dto.thumnail = await _postJobRepository.getThumnailJob(dto.PostId);
            }
            return pdto;
        }

        public async Task<PostJobDTOForList>  GetPostJobById(int id) 
        {
            PostJob postJob = await _postJobRepository.GetPostJobById(id);
            PostJobDTOForList postJobDTO = _mapper.Map<PostJobDTOForList>(postJob);
            return postJobDTO;
        }
    }
}
