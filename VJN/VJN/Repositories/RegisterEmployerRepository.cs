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
            var c1 = await _context.RegisterEmployers.Where(re => re.UserId == employer.UserId && re.Status == 1).AnyAsync();
            var c2 = await _context.Users.Where(re => re.UserId == employer.UserId).Where(u => u.RoleId == 2).AnyAsync();
            if (c) {
                return -1;
            }
            else if(c1||c2)
            {
                return -2;
            }

            _context.RegisterEmployers.Add(employer);
            await _context.SaveChangesAsync();
            return employer.RegisterEmployerId;
        }

        public async Task<bool> AcceptRegisterEmployer(int id)
        {
            // Find RegisterEmployer entry and check for existence
            var re = await _context.RegisterEmployers.FindAsync(id);
            if (re == null)
            {
                return false;
            }

            // Set status to approved
            re.Status = 1;
            _context.RegisterEmployers.Update(re);

            // Verify UserId and update user role
            var _user = await _context.Users.FindAsync(re.UserId);
            if (_user != null)
            {
                _user.RoleId = 2;
                _context.Users.Update(_user);
                await _context.SaveChangesAsync();
                return true;
            }
             
            // Return false if user was not found
            return false;
        }


        public async Task<bool> RejectRegisterEmployer(int id, string reason)
        {
            // Find RegisterEmployer entry and check for existence
            var re = await _context.RegisterEmployers.FindAsync(id);
            if (re == null)
            {
                return false;
            }

            // Set status to reject
            re.Status = 2;
            re.Reason = reason;
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

        public async Task<RegisterEmployer> GetRegisterEmployerByUserID(int id)
        {
            var register = await _context.RegisterEmployers.OrderByDescending(re=>re.CreateDate).Where(re=>re.UserId==id).FirstOrDefaultAsync();
            return register;
        }
    }
}
