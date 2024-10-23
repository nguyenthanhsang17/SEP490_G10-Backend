using MailKit.Search;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            var query = _context.PostJobs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(s.JobTitle))
                query = query.Where(j => j.JobTitle.Contains(s.JobTitle));

            if (!string.IsNullOrWhiteSpace(s.JobDescription))
                query = query.Where(j => j.JobDescription.Contains(s.JobDescription));

            if (s.SalaryTypesId!=0)
                query = query.Where(j => j.SalaryTypesId == s.SalaryTypesId.Value);

            // Xử lý tìm kiếm lương
            if (s.RangeSalaryMin.HasValue || s.RangeSalaryMax.HasValue)
            {
                query = query.Where(j =>
                    (j.Salary.HasValue &&
                     (!s.RangeSalaryMin.HasValue || j.Salary >= s.RangeSalaryMin) &&
                     (!s.RangeSalaryMax.HasValue || j.Salary <= s.RangeSalaryMax))
                );
            }

            if (!string.IsNullOrWhiteSpace(s.Address))
                query = query.Where(j => j.Address.Contains(s.Address));

            if (s.distance.HasValue && s.Latitude.HasValue && s.Longitude.HasValue)
            {
                query = query.Where(j =>
                    (
                        Math.Acos(Math.Sin((double)s.Latitude.Value * Math.PI / 180) *
                                  Math.Sin((double)j.Latitude * Math.PI / 180) +
                                  Math.Cos((double)s.Latitude.Value * Math.PI / 180) *
                                  Math.Cos((double)j.Latitude * Math.PI / 180) *
                                  Math.Cos(((double)s.Longitude.Value - (double)j.Longitude) * Math.PI / 180)) * 6371
                    ) <= s.distance.Value
                );
            }

            if (s.IsUrgentRecruitment.HasValue)
                query = query.Where(j => j.IsUrgentRecruitment == s.IsUrgentRecruitment.Value);

            if (s.JobCategoryId!=0)
                query = query.Where(j => j.JobCategoryId == s.JobCategoryId.Value);
            if (s.SortNumberApplied != 0)
            {
                if(s.SortNumberApplied == -1)
                {
                    query = query.OrderByDescending(j => j.ApplyJobs.Count());
                }
                else
                {
                    query = query.OrderBy(j => j.ApplyJobs.Count());
                }
            }


            // Sắp xếp theo số lượng người đã apply và chỉ lấy Post_Id
            var result = await query
                .Select(j => j.PostId)
                .ToListAsync();

            var sql = query.ToQueryString(); // Bạn có thể cần thêm extension method này
            Console.WriteLine("sang: "+sql);
            Console.WriteLine(sql);
            return result;
        }

        public async Task<string> getThumnailJob(int id)
        {
            var urls = await _context.ImagePostJobs.Where(im => im.PostId == id).ToListAsync();
            var idurl = urls.FirstOrDefault().ImageId;

            var image = await _context.MediaItems.Where(mi => mi.Id == id).SingleOrDefaultAsync();
            return image.Url;
        }


        public async Task<IEnumerable<PostJob>> jobSearchResults(IEnumerable<int> jobIds)
        {
            var jobs = await _context.PostJobs.Include(j => j.Author).Include(j => j.JobCategory).Include(j => j.SalaryTypes).Where(u => jobIds.Contains(u.PostId)).ToListAsync();
            return jobs;
        }

        public async Task<PostJob> getJostJobByID(int id)
        {
            var job = await _context.PostJobs.Include(j=>j.Author).Include(j=>j.JobCategory).Include(j=>j.SalaryTypes).Where(j=>j.PostId==id).SingleOrDefaultAsync();
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
            var imgs = await _context.ImagePostJobs.Include(imj=>imj.Image).Where(imj=>imj.PostId==jid).Select(imj=>imj.Image.Url).ToListAsync();
            return imgs;
        }

        public async Task<bool> GetisAppliedJob(int jid, int userid)
        {
            return await _context.ApplyJobs.Where(aj=>aj.JobSeekerId==userid&&aj.PostId==jid).AnyAsync();
        }

        public async Task<bool> GetisWishJob(int jid, int userid)
        {
            return await _context.WishJobs.Where(aj => aj.JobSeekerId == userid && aj.PostJobId == jid).AnyAsync();
        }
    }
}
