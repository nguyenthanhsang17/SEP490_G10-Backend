using VJN.Models;
using VJN.ModelsDTO.RegisterEmployer;

namespace VJN.Services
{
    public interface IRegisterEmployerService
    {
        public Task<int> RegisterEmployer(VerifyEmployerAccountDTO dto, int userid);

        public Task<IEnumerable<RegisterEmployer>> getRegisterEmployerByStatus(int status);
        public Task<RegisterEmployer> getRegisterEmployerByID(int id);
        public Task<bool> AcceptRegisterEmployer(int status);
        public Task<bool> RejectRegisterEmployer(int status, string reason);
    }
}
