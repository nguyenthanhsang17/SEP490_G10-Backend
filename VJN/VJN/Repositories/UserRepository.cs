using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly VJNDBContext _context;     
        public UserRepository(VJNDBContext context) {
            _context = context;
        }

        public async Task<User> findById(int id)
        {
            User user = null;
            try
            {
                user = await _context.Users.Include(u => u.Role).Where(u=>u.UserId==id).SingleOrDefaultAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public async Task<IEnumerable<User>> getAllUser()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return users;
        }

        public async Task<User> Login(string Username, string Password)
        {
            var user =await _context.Users.Where(u=>u.UserName.Equals(Username)&&u.Password.Equals(Password)).Include(u=>u.Role).SingleOrDefaultAsync();
            return user;
        }
    }
}
