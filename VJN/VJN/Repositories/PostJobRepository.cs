using MailKit.Search;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Text;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;

namespace VJN.Repositories
{

    public class PostJobRepository : IPostJobRepository
    {
        private readonly VJNDBContext _context;
        const double EarthRadiusKm = 6371.0;
        public PostJobRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostJob>> GetPorpularJob()
        {
            var jobTopIds = await _context.PostJobs.GroupBy(pj => pj.PostId).Select(g => new
            {
                PostId = g.Key,
                ApplicantCount = _context.ApplyJobs.Count(aj => aj.PostId == g.Key)
            })
            .OrderByDescending(x => x.ApplicantCount)
            .Take(3)
            .Select(x => x.PostId)
            .ToListAsync();

            var topPosts = await _context.PostJobs.Include(pj => pj.Author).Include(j => j.JobCategory).Include(j => j.SalaryTypes)
                .Where(pj => jobTopIds.Contains(pj.PostId))
                .ToListAsync();

            return topPosts;
        }
        public double Haversine(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double dLat = DegreesToRadians((double)(lat2 - lat1));
            double dLon = DegreesToRadians((double)(lon2 - lon1));

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians((double)lat1)) * Math.Cos(DegreesToRadians((double)lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public async Task<IEnumerable<int>> SearchJobPopular(PostJobSearch s)
        {
            string sql = "SELECT * FROM PostJob p WHERE 1=1 AND p.ExpirationDate > GETDATE() ";

            if (!string.IsNullOrEmpty(s.JobKeyWord))
            {
                sql = sql + $" and dbo.RemoveDiacritics(p.JobTitle)  LIKE '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}')+'%'";
                sql = sql + $" OR dbo.RemoveDiacritics(JobDescription) like '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}%')+'%' ";
            }
            if(s.SalaryTypesId != 0)
            {
                sql = sql + $" and salary_types_id = {s.SalaryTypesId}";
            }
            if (!string.IsNullOrEmpty(s.Address))
            {
                sql = sql + $" and dbo.RemoveDiacritics(p.Address) like '%'+ dbo.RemoveDiacritics(N'{s.Address}')+'%'";
            }
            if(s.RangeSalaryMin.HasValue)
            {
                sql = sql + $" and Salary >= {s.RangeSalaryMin}";
            }
            if (s.RangeSalaryMax.HasValue)
            {
                sql = sql + $" and Salary <= {s.RangeSalaryMax} ";
            }
            if (s.IsUrgentRecruitment!=-1)
            {
                sql = sql + $" and p.IsUrgentRecruitment = {s.IsUrgentRecruitment}";
            }

            if(s.JobCategoryId!=0)
            {
                sql = sql + $" and p.JobCategory_Id = {s.JobCategoryId}";
            }
            if (s.distance.HasValue && s.Latitude.HasValue && s.Longitude.HasValue)
            {
                sql = sql + $" and (6371 * ACOS(COS(RADIANS({s.Latitude})) * COS(RADIANS(p.latitude)) * COS(RADIANS(p.longitude) - RADIANS({s.Longitude})) +SIN(RADIANS({s.Latitude})) * SIN(RADIANS(p.latitude)))) <= {s.distance}";
            }
            if (s.SortNumberApplied != 0)
            {
                sql = sql + " GROUP BY p.Post_Id, p.JobTitle, p.JobDescription, p.salary_types_id, p.Salary, p.NumberPeople, p.Address, p.latitude, p.longitude, p.AuthorId, p.CreateDate, p.ExpirationDate, p.Status, p.censor_Id, p.censor_Date, p.IsUrgentRecruitment, p.JobCategory_Id";
                if(s.SortNumberApplied > 0)
                {
                    sql = sql + " order by COUNT(p.Post_Id) ";
                }
                else
                {
                    sql = sql + " order by COUNT(p.Post_Id) desc";
                }
            }
            Console.WriteLine(sql);
            

            var query = _context.PostJobs.FromSqlRaw(sql);
            var results = await query.ToListAsync();
            var id = results.Select(u => u.PostId);
            return id;
        }

        public async Task<string> getThumnailJob(int id)
        {
            var urls = await _context.ImagePostJobs.Where(im => im.PostId == id).ToListAsync();
            if (urls.Count == 0)
            {
                return "";
            }
            var idurl = urls.FirstOrDefault().ImageId;

            var image = await _context.MediaItems.Where(mi => mi.Id == id).SingleOrDefaultAsync();

            return image.Url;
        }


        public async Task<IEnumerable<PostJob>> jobSearchResults(IEnumerable<int> jobIds)
        {
            var jobs = await _context.PostJobs.Include(j => j.Author).Include(j => j.JobCategory).Include(j => j.SalaryTypes).Include(j => j.ImagePostJobs).ThenInclude(img => img.Image).Where(u => jobIds.Contains(u.PostId)).Include(j=>j.ApplyJobs).ToListAsync();
            return jobs;
        }

        public async Task<PostJob> getJostJobByID(int id)
        {
            var job = await _context.PostJobs.Include(j => j.Author).Include(j => j.JobCategory).Include(j => j.SalaryTypes).Include(j => j.ImagePostJobs).ThenInclude(img => img.Image).Where(j => j.PostId == id).SingleOrDefaultAsync();
            return job;
        }

        public async Task<int> CountApplyJob(int jobId)
        {
            return await _context.ApplyJobs.CountAsync(a => a.PostId == jobId);
        }

        public async Task<PostJob> GetPostJobById(int id)
        {
            PostJob postJob = null;
            try
            {
                postJob = await _context.PostJobs.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return postJob;
        }

        public async Task<IEnumerable<string>> getAllImageJobByJobId(int jid)
        {
            var imgs = await _context.ImagePostJobs.Include(imj => imj.Image).Where(imj => imj.PostId == jid).Select(imj => imj.Image.Url).ToListAsync();
            return imgs;
        }

        public async Task<bool> GetisAppliedJob(int jid, int userid)
        {
            return await _context.ApplyJobs.Where(aj => aj.JobSeekerId == userid && aj.PostId == jid).AnyAsync();
        }

        public async Task<bool> GetisWishJob(int jid, int userid)
        {
            return await _context.WishJobs.Where(aj => aj.JobSeekerId == userid && aj.PostJobId == jid).AnyAsync();
        }

        public async Task<bool> ChangeStatusPostJob(int jobID, int status)
        {
            var job = await _context.PostJobs.Where(j => j.PostId == jobID).SingleOrDefaultAsync();
            if (job == null)
            {
                return false;
            }
            job.Status = status;
            _context.Entry(job).State = EntityState.Modified;
            int i = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreatePostJob(PostJob postJob)
        {
            try
            {
                _context.PostJobs.Add(postJob);
                await _context.SaveChangesAsync();
                return postJob.PostId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<int>> GetPostJobCreatedByEmployerID(int employerID, PostJobSearchEmployer s)
        {
            string sql = $"SELECT * FROM PostJob p WHERE 1=1 and AuthorId = {employerID} ";

            if (!string.IsNullOrEmpty(s.JobKeyWord))
            {
                sql = sql + $" and dbo.RemoveDiacritics(p.JobTitle)  LIKE '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}')+'%'";
                sql = sql + $" OR dbo.RemoveDiacritics(JobDescription) like '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}%')+'%' ";
            }
            if (s.SalaryTypesId != 0)
            {
                sql = sql + $" and salary_types_id = {s.SalaryTypesId} ";
            }
            if (s.RangeSalaryMin.HasValue)
            {
                sql = sql + $" and Salary >= {s.RangeSalaryMin} ";
            }
            if (s.RangeSalaryMax.HasValue)
            {
                sql = sql + $" and Salary <= {s.RangeSalaryMax} ";
            }
            if (s.IsUrgentRecruitment!=-1)
            {
                sql = sql + $" and p.IsUrgentRecruitment = {s.IsUrgentRecruitment} ";
            }

            if (s.Status != -1)
            {
                sql = sql + $" and p.Status = {s.Status} "; 
            }

            if (s.JobCategoryId != 0)
            {
                sql = sql + $" and p.JobCategory_Id = {s.JobCategoryId} ";
            }
            if (s.SortNumberApplied != 0)
            {
                sql = sql + " GROUP BY p.Post_Id, p.JobTitle, p.JobDescription, p.salary_types_id, p.Salary, p.NumberPeople, p.Address, p.latitude, p.longitude, p.AuthorId, p.CreateDate, p.ExpirationDate, p.Status, p.censor_Id, p.censor_Date, p.IsUrgentRecruitment, p.JobCategory_Id";
                if (s.SortNumberApplied > 0)
                {
                    sql = sql + " order by COUNT(p.Post_Id) ";
                }
                else
                {
                    sql = sql + " order by COUNT(p.Post_Id) desc ";
                }
            }
            Console.WriteLine(sql);


            var query = _context.PostJobs.FromSqlRaw(sql);
            var results = await query.ToListAsync();
            var id = results.Select(u => u.PostId);
            return id;
        }
    }
}
