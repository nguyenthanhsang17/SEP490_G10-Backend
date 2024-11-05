
using VJN.Repositories;

namespace VJN.Services
{
    public class RegisterEmployerMediaService : IRegisterEmployerMediaService
    {
        private readonly IRegisterEmployerMediaRepository _registerEmployerMediaRepository;

        public RegisterEmployerMediaService(IRegisterEmployerMediaRepository registerEmployerMediaRepository)
        {
            _registerEmployerMediaRepository = registerEmployerMediaRepository;
        }

        public async Task<bool> CreateRegisterEmployerMedia(int registerID, List<int> imageid)
        {
            var c = await _registerEmployerMediaRepository.CreateRegisterEmployerMedia(registerID, imageid);
            return c;
        }
    }
}
