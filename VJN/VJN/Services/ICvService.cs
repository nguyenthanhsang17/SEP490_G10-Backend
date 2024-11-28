using VJN.Models;
using VJN.ModelsDTO.CvDTOs;

namespace VJN.Services
{
    public interface ICvService
    {
        public Task<IEnumerable<CvDTODetail>> GetCvByUserID(int user);
        public Task<bool> UpdateCV(List<CvUpdateDTO> cvs, int userid);

        public  Task<IEnumerable<CvDTODetail>> GetCvAllcv();
        public Task<bool> UpdateCv(CvDTODetail cv);
        public Task<bool> DeleteCV(int cvid);
    }
}
