using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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

        public async Task<bool> AcceptRegisterEmployer(int id)
        {
            var re = await _context.RegisterEmployers.FindAsync(id);
            if (re == null)
            {
                return false; 
            }
            re.Status = 1; 
            _context.RegisterEmployers.Update(re);
            var _user = await _context.Users.FindAsync(id);
            _user.RoleId = 3;
            _context.Users.Update(_user);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<bool> RejectRegisterEmployer(int id, string reason)
        {
            var re = await _context.RegisterEmployers.FindAsync(id);
            if (re == null)
            {
                return false; 
            }
            re.Status = 2; 
            // Làm gì đó với reason
            _context.RegisterEmployers.Update(re);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<IEnumerable<RegisterEmployer>> getRegisterEmployerByStatus(int status)
        {
            if (status == -1)
            {
                return await _context.RegisterEmployers
                    .Include(rg => rg.RegisterEmployerMedia)
                    .ThenInclude(rgm => rgm.Media)
                    .Include(rg=>rg.User).ThenInclude(u=>u.AvatarNavigation)
                    .ToListAsync();
            }
            return await _context.RegisterEmployers.Where(x => x.Status == status)
                .Include(rg => rg.RegisterEmployerMedia)
                    .ThenInclude(rgm => rgm.Media)
                    .Include(rg => rg.User).ThenInclude(u => u.AvatarNavigation)
                    .ToListAsync();
        }

        public async Task<RegisterEmployer> getRegisterEmployerByid(int id)
        {
            return await _context.RegisterEmployers
                .Include(rg => rg.RegisterEmployerMedia).ThenInclude(rgm => rgm.Media)
                .Include(rg => rg.User).ThenInclude(u => u.AvatarNavigation)
                .FirstOrDefaultAsync(rg => rg.RegisterEmployerId == id);
        }
    }
}
