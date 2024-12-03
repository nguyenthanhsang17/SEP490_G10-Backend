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
                user = await _context.Users.Include(u => u.Role).Include(u=>u.AvatarNavigation).Include(u=>u.CurrentJobNavigation).Where(u=>u.UserId==id).SingleOrDefaultAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public async Task<IEnumerable<User>> getAllUser()
        {
            var users = await _context.Users.Include(u => u.Role).Include(u => u.AvatarNavigation).Include(u => u.CurrentJobNavigation).ToListAsync();
            return users;
        }

        public async Task<User> Login(string Username, string Password)
        {
            var user =await _context.Users.Where(u=>u.Email.Equals(Username)&&u.Password.Equals(Password)).Include(u=>u.Role).SingleOrDefaultAsync();
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

        public async Task<int> CreateUser(User user)
        {
            if(user == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return 1;
                }catch (Exception ex) {
                    throw new Exception(ex.Message);
                }

            }
        }

        public async Task<int> UpdateStatus(int uid, int status)
        {
            User user = null;
            try
            {
                user = await _context.Users.Where(u=>u.UserId==uid).SingleOrDefaultAsync();
                if(user == null)
                {
                    return 0;
                }
                user.Status = status;
                _context.Entry(user).State = EntityState.Modified;
                int i= await _context.SaveChangesAsync();
                return i>=1?1:0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckEmailExits(string Email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Equals(Email));
        }

        public async Task<bool> UpdateProfile(int v, User model)
        {
            User u = null;
            try
            {
                u = await _context.Users.FindAsync(v);
                if (model.Avatar.HasValue)
                {
                    u.Avatar = model.Avatar.Value;
                }

                if (!string.IsNullOrEmpty(model.FullName))
                {
                    u.FullName = model.FullName;
                }

                if (model.Age.HasValue)
                {
                    u.Age = model.Age.Value;
                }

                if (!string.IsNullOrEmpty(model.Phonenumber))
                {
                    u.Phonenumber = model.Phonenumber;
                }

                if (model.CurrentJob.HasValue)
                {
                    u.CurrentJob = model.CurrentJob.Value;
                }

                if (!string.IsNullOrEmpty(model.Description))
                {
                    u.Description = model.Description;
                }

                if (!string.IsNullOrEmpty(model.Address))
                {
                    u.Address = model.Address;
                }
                if (model.Gender.HasValue)
                {
                    u.Gender = model.Gender.Value;
                }
                _context.Entry(u).State = EntityState.Modified;
                int i = await _context.SaveChangesAsync();
                return i >= 1;
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdatePassword(int userid, string password)
        {
            User user = await _context.Users.Where(u=>u.UserId==userid).SingleOrDefaultAsync();
            if (user != null)
            {
                var diffTime = DateTime.Now - user.SendCodeTime;
                if (diffTime.HasValue && Math.Abs(diffTime.Value.TotalMinutes) >= 5)
                {
                    return false;
                }
                user.Password = password;
                _context.Entry(user).State = EntityState.Modified;
                int i = await _context.SaveChangesAsync();
                return i >= 1;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserVerifycode(User user)
        {
            user.VerifyCode = null;
            user.SendCodeTime = null;
            _context.Entry(user).State = EntityState.Modified;
            var i = await _context.SaveChangesAsync();
            return i >= 1;
        }

        public async Task<bool> CheckOtpExits(string otp)
        {
            return await _context.Users.AnyAsync(u => u.VerifyCode.Equals(otp));
        }

        public async Task<User> GetUserByOtp(string otp)
        {
            User u = null;
            try
            {
                u = await _context.Users.Where(u=>u.VerifyCode.Equals(otp)).SingleOrDefaultAsync();
                return u;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertOTP(int userid, string otp)
        {
            var user = await _context.Users.Where(u=>u.UserId==userid).SingleOrDefaultAsync();
            user.VerifyCode = otp;
            user.SendCodeTime = DateTime.Now;
            _context.Entry(user).State = EntityState.Modified;
            var i = await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUserWithoutAdmin()
        {
            var users = await _context.Users.Include(u => u.Role).Include(u => u.AvatarNavigation).Include(u => u.CurrentJobNavigation).Where(u=>u.RoleId!=4).ToListAsync();
            return users;
        }
    }
}
