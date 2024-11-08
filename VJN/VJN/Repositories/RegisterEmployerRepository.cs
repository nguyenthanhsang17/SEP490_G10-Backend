using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class RegisterEmployerRepository : IRegisterEmployerRepository
    {
        private readonly VJNDBContext _context;

        public RegisterEmployerRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<int> RegisterEmployer(RegisterEmployer employer)
        {

            var c = await _context.RegisterEmployers.Where(re=>re.UserId==employer.UserId&&re.Status==0).AnyAsync();
            var c1 = await _context.RegisterEmployers.Where(re => re.UserId == employer.UserId && re.Status == 2).AnyAsync();
            if (c) {
                return -1;
            }
            else if(c1)
            {
                return -2;
            }

            _context.RegisterEmployers.Add(employer);
            await _context.SaveChangesAsync();
            return employer.RegisterEmployerId;
        }
    }
}
