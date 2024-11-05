using VJN.ModelsDTO.RegisterEmployer;

namespace VJN.Services
{
    public interface IRegisterEmployerService
    {
        public Task<int> RegisterEmployer(VerifyEmployerAccountDTO dto, int userid);
    }
}
