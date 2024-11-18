using VJN.Models;

namespace VJN.Repositories
{
    public interface IRegisterEmployerRepository
    {
        public Task<int> RegisterEmployer(RegisterEmployer employer);
        public Task<IEnumerable<RegisterEmployer>> getRegisterEmployerByStatus(int status);
        public Task<RegisterEmployer> getRegisterEmployerByid(int id);
        public Task<bool> AcceptRegisterEmployer(int status);
        public Task<bool> RejectRegisterEmployer(int status, string reason);
        public Task<RegisterEmployer> GetRegisterEmployerByUserID(int id);
    }
}
