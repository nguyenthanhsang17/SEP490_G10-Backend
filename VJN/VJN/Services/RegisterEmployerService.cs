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
    }
}
