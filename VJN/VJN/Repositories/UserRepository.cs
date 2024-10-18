using Microsoft.EntityFrameworkCore;
using VJN.Models;
using static System.Net.WebRequestMethods;

namespace VJN.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly VJNDBContext _context;     
        public UserRepository(VJNDBContext context) {
            _context = context;
        }

        public async Task<int> ChangePassword(string NewPassword, int userid)
        {
            User user = null;
            try
            {
                user = await _context.Users.Where(u => u.UserId == userid).SingleOrDefaultAsync();
                if(user == null)
                {
                    return 0;
                }
                else
                {
                    user.Password = NewPassword;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return 1;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetUserIdEmailExits(string Email)
        {
            User user = null;
            try
            {
                user = await _context.Users.Where(u=>u.Email.Equals(Email)).SingleOrDefaultAsync();
                if(user == null)
                {
                    return 0;
                }else {
                    return user.UserId;
                }
            }catch (Exception ex) {
                throw new Exception(ex.Message);
            }
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

        public async Task<int> UpdateOtpUser(int userid, string otp)
        {
            
            User user = null;
            try
            {
                user = await _context.Users.FindAsync(userid);
                if(user == null)
                {
                    return 0;
                }
                else
                {
                    user.VerifyCode = otp;
                    user.SendCodeTime = DateTime.Now;
                    _context.Entry(user).State = EntityState.Modified;
                    int i = await _context.SaveChangesAsync();
                    if (i > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = null;
            try
            {
                user = await _context.Users.Where(u=>u.Email.Equals(email)).SingleOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
