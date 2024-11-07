using VJN.Models;
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
                NameCv = cv.NameCv,
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

        public async Task<bool> UpdateCV(List<CvUpdateDTO> cvsdto, int userid)
        {
            List<Cv> cvs = new List<Cv>();
            foreach (CvUpdateDTO cv in cvsdto)
            {
                var model = new Cv()
                {
                    NameCv = cv.NameCv,
                    UserId = userid,
                };
                List<ItemOfCv> modelITs = new List<ItemOfCv>();
                foreach (var item in cv.ItemOfCvs)
                {
                    var modelIT = new ItemOfCv()
                    {
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                    };
                    modelITs.Add(modelIT);
                }
                model.ItemOfCvs = modelITs;
                cvs.Add(model);
            }
            var c = await _cvRepository.UpdateCV(cvs, userid);
            return c;
        }
    }
}
