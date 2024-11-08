
using VJN.Models;

namespace VJN.Repositories
{
    public class RegisterEmployerMediaRepository : IRegisterEmployerMediaRepository
    {

        private readonly VJNDBContext _context;

        public RegisterEmployerMediaRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRegisterEmployerMedia(int registerID, List<int> imageid)
        {
            foreach (var image in imageid)
            {
                var rm = new RegisterEmployerMedium();
                rm.RegisterEmployerId = registerID;
                rm.MediaId = image;
                _context.RegisterEmployerMedia.Add(rm);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
