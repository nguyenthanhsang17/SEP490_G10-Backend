using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class CvRepository : ICvRepository
    {
        private readonly VJNDBContext _context;
        public CvRepository(VJNDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cv>> GetCvByUserID(int user)
        {
            var cvs = await _context.Cvs.Where(c => c.UserId == user).Include(c=>c.ItemOfCvs).ToListAsync();
            return cvs;
        }
    }
}
