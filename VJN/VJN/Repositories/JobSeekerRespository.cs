using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.FavoriteListDTOs;

namespace VJN.Repositories
{
    public class JobSeekerRespository : IJobSeekerRespository
    {
        private readonly VJNDBContext _context;
        private const int PageSize = 6;
        public JobSeekerRespository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFavorite(FavoriteList model)
        {
            var check = await _context.FavoriteLists.Where(x=> x.EmployerId== model.EmployerId&& x.JobSeekerId==model.JobSeekerId).AnyAsync();
            if(model.EmployerId == model.JobSeekerId)
            {
                return false;
            }
            if (check)
            {
                return false;
            }

            
            _context .FavoriteLists.Add(model);
            int i = await _context.SaveChangesAsync();
            return i>=1;

        }

        public async Task<bool> DeleteFavorite(int JobseekerID, int userid)
        {
            var fl = await _context.FavoriteLists.Where(x=>x.EmployerId==userid&&x.JobSeekerId==JobseekerID).SingleOrDefaultAsync();
            if(fl==null)
            {
                return false;   
            }

            _context.FavoriteLists.Remove(fl);
            var i = await _context.SaveChangesAsync();
            return (i>=1);
        }

        public async Task<IEnumerable<int>> GetAllFavoriteId(FavoriteListSearch s, int userid)
        {
            var sql = $"SELECT u.* FROM Favorite_List fl JOIN [User] u ON fl.JobSeekerId = u.User_Id LEFT JOIN \r\nApplyJob aj ON fl.JobSeekerId = aj.JobSeeker_Id and (aj.Status = 3 or aj.Status = 4)\r\nWHERE fl.EmployerId = {userid}";

            if (!string.IsNullOrEmpty(s.Description))
            {
                sql = sql + $" and dbo.RemoveDiacritics(fl.Description)  LIKE '%'+ dbo.RemoveDiacritics(N'{s.Description}')+'%'";
            }
            sql = sql + " GROUP BY u.User_Id, u.Email, u.Avatar, u.FullName, u.Password, u.Age, u.Phonenumber, u.CurrentJob, u.Description, u.Address,  u.Balance, u.Status, u.Gender, u.SendCodeTime, u.VerifyCode, u.Role_Id ";
            if(s.sort.HasValue&&s.sort==1) //sort ==0 ko con sỏrt == 1 
            {
                sql = sql + " order by COUNT(aj.id) desc";
            }
            else
            {
                sql = sql + " order by u.User_Id desc ";
            }

            Console.WriteLine(sql);
            var query = _context.Users.FromSqlRaw(sql);
            var results = await query.ToListAsync();
            var id = results.Select(u => u.UserId);
            return id;
        }

        public async Task<IEnumerable<User>> GetAllFavorite(IEnumerable<int> ids)
        {
            List<User> users = new List<User>();
            foreach (var id in ids)
            {
                var u  = await _context.Users.Include(u=>u.FavoriteListJobSeekers).Include(u=>u.AvatarNavigation).Include(x=>x.CurrentJobNavigation).Include(x=>x.ApplyJobs).Where(u => u.UserId == id).SingleOrDefaultAsync();
                users.Add(u);
            }
            return users;
        }

        public async Task<User> GetJobSeekerByIserID(int userID)
        {
            var user =  await _context.Users.Where(u=>u.UserId==userID).Include(u=>u.ApplyJobs).Include(u=>u.CurrentJobNavigation).Include(u=>u.AvatarNavigation).SingleOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<Cv>> GetCVByUserId(int userID)
        {
            var cvs = await _context.Cvs.Where(cv => cv.UserId == userID).Include(cv=>cv.ItemOfCvs).ToListAsync();
            return cvs;
        }

        public Task<IEnumerable<int>> GetAllJobSeeker()
        {
            throw new NotImplementedException();
        }
    }
}
