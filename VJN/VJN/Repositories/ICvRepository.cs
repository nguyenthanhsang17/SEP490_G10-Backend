using VJN.Models;

namespace VJN.Repositories
{
    public interface ICvRepository
    {
        public Task<IEnumerable<Cv>> GetCvByUserID(int user);
        public  Task<IEnumerable<Cv>> GetCvAllcv();
        public Task<bool> UpdateCV(List<Cv> cvs, int userid);
        public Task<int> CreateCv(Cv cv);
        public Task<int> UpdateCv(Cv cv);
        public Task<bool> DeleteCv(int cvid);
    }
}
