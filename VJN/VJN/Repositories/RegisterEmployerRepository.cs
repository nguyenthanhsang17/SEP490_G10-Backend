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

            var c = await _context.Users.Where(x => x.UserId==employer.UserId&&x.Status==2).AnyAsync();

            if(c) {
                return -1;
            }

            _context.RegisterEmployers.Add(employer);
            await _context.SaveChangesAsync();
            return employer.RegisterEmployerId;
        }
    }
}
