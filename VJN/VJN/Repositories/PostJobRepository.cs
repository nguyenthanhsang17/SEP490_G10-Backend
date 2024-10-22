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

            var sqlQuery = @"
        SELECT pj.PostId, COUNT(aj.PostId) AS NumberOfApplicants
        FROM PostJobs pj
        LEFT JOIN ApplyJobs aj ON pj.PostId = aj.PostId
        WHERE (STRING_LENGTH(@JobTitle) = 0 OR LOWER(pj.JobTitle) LIKE '%' + LOWER(@JobTitle) + '%')
        AND (STRING_LENGTH(@JobDescription) = 0 OR LOWER(pj.JobDescription) LIKE '%' + LOWER(@JobDescription) + '%')
        AND (@SalaryTypesId = 0 OR @SalaryTypesId = pj.SalaryTypesId)
        AND (pj.ExpirationDate >= GETDATE())
        AND (pj.Status = 2)
        AND (@JobCategoryId = 0 OR @JobCategoryId = pj.JobCategoryId)
        GROUP BY pj.PostId
        ORDER BY NumberOfApplicants DESC";

            var jobIds = await _context.PostJobs
                .FromSqlRaw(sqlQuery,
                    new SqlParameter("@JobTitle", s.JobTitle ?? string.Empty),
                    new SqlParameter("@JobDescription", s.JobDescription ?? string.Empty),
                    new SqlParameter("@SalaryTypesId", s.SalaryTypesId),
                    new SqlParameter("@JobCategoryId", s.JobCategoryId))
                .Select(j => j.PostId)
                .ToListAsync();

            return jobIds;

            return jobIds;
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

    }
}
