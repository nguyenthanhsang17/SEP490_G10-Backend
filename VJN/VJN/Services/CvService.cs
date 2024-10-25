using VJN.ModelsDTO.CvDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class CvService : ICvService
    {
        private readonly ICvRepository _cvRepository;

        public CvService(ICvRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        public async Task<IEnumerable<CvDTODetail>> GetCvByUserID(int user)
        {
            var cv = await _cvRepository.GetCvByUserID(user);
            var cvdto = cv.Select(cv => new CvDTODetail
            {
                CvId = cv.CvId,
                UserId = cv.UserId,
                ItemOfCvs = cv.ItemOfCvs.Select(it => new ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView
                {
                    ItemOfCvId = it.ItemOfCvId,
                    CvId = cv.CvId,
                    ItemDescription = it.ItemDescription,
                    ItemName = it.ItemName,
                }).ToList(),
            }).ToList();
            return cvdto;
        }
    }
}
