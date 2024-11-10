using SQLitePCL;
using VJN.Models;
using VJN.ModelsDTO.RegisterEmployer;
using VJN.Repositories;

namespace VJN.Services
{
    public class RegisterEmployerService : IRegisterEmployerService
    {
        private readonly IRegisterEmployerRepository _registerEmployerRepository;

        public RegisterEmployerService(IRegisterEmployerRepository registerEmployerRepository)
        {
            _registerEmployerRepository = registerEmployerRepository;
        }

        public async Task<int> RegisterEmployer(VerifyEmployerAccountDTO dto, int u)
        {
            var rg = new RegisterEmployer()
            {
                UserId = u,
                BussinessName = dto.BussinessName,
                BussinessAddress = dto.BussinessAddress,
                CreateDate = DateTime.Now,
                Status = 0,
            };

            int id = await _registerEmployerRepository.RegisterEmployer(rg);
            return id;
        }

        public async Task<bool> RejectRegisterEmployer(int status, string reason)
        {
            return await _registerEmployerRepository.RejectRegisterEmployer(status, reason);
        }

        public async Task<bool> AcceptRegisterEmployer(int status)
        {
            return await _registerEmployerRepository.AcceptRegisterEmployer(status);
        }

        public async Task<IEnumerable<RegisterEmployer>> getRegisterEmployerByStatus(int status)
        {
            return await _registerEmployerRepository.getRegisterEmployerByStatus(status);
        }

        public async Task<RegisterEmployer> getRegisterEmployerByID(int id)
        {
            return await _registerEmployerRepository.getRegisterEmployerByid(id);
        }
    }
}
