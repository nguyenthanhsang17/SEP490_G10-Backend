﻿using MailKit.Search;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Text;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;

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
            var jobTopIds = await _context.PostJobs.Where(p=>p.Status==2&&p.ExpirationDate>=DateTime.Now).GroupBy(pj => pj.PostId).Select(g => new
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
            string sql = "";
            if (s.SortNumberApplied != 0)
            {
                sql = "SELECT p.* FROM PostJob p left join ApplyJob aj on p.Post_Id = aj.Post_Id WHERE 1=1 AND p.ExpirationDate > GETDATE() AND p.Status = 2 ";
            }
            else
            {
                sql = "SELECT p.* FROM PostJob p WHERE 1=1 AND p.ExpirationDate > GETDATE() AND p.Status = 2 ";
            }


            if (!string.IsNullOrEmpty(s.JobKeyWord))
            {
                sql = sql + $" and ( dbo.RemoveDiacritics(p.JobTitle)  LIKE '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}')+'%'";
                sql = sql + $" OR dbo.RemoveDiacritics(JobDescription) like '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}%')+'%' ) ";
            }
            if (s.SalaryTypesId != 0)
            {
                sql = sql + $" and salary_types_id = {s.SalaryTypesId}";
            }
            if (!string.IsNullOrEmpty(s.Address))
            {
                sql = sql + $" and dbo.RemoveDiacritics(p.Address) like '%'+ dbo.RemoveDiacritics(N'{s.Address}')+'%'";
            }

            if (s.JobCategoryId != 0)
            {
                sql = sql + $" and p.JobCategory_Id = {s.JobCategoryId}";
            }
            if (s.distance.HasValue && s.Latitude.HasValue && s.Longitude.HasValue)
            {
                sql = sql + $" and (6371 * ACOS(COS(RADIANS({s.Latitude})) * COS(RADIANS(p.latitude)) * COS(RADIANS(p.longitude) - RADIANS({s.Longitude})) +SIN(RADIANS({s.Latitude})) * SIN(RADIANS(p.latitude)))) <= {s.distance}";
            }
            if (s.SortNumberApplied != 0)
            {
                sql = sql + " GROUP BY p.Post_Id, p.JobTitle, p.JobDescription, p.salary_types_id, p.Salary, p.NumberPeople, p.Address, p.latitude, p.longitude, p.AuthorId, p.CreateDate, p.ExpirationDate, p.Status, p.censor_Id, p.censor_Date, p.Reason, p.IsUrgentRecruitment, p.JobCategory_Id, p.time ";
                if (s.SortNumberApplied > 0)
                {
                    sql = sql + " order by COUNT(aj.id) ";
                }
                else
                {
                    sql = sql + " order by COUNT(aj.id) desc";
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
            Console.WriteLine(urls.Count);
            if (urls.Count == 0)
            {
                return "";
            }
            var idurl = urls.FirstOrDefault().ImageId;
            Console.WriteLine(idurl);
            var image = await _context.MediaItems.Where(mi => mi.Id == idurl).SingleOrDefaultAsync();

            return image.Url;
        }


        public async Task<IEnumerable<PostJob>> jobSearchResults(IEnumerable<int> jobIds)
        {
            List<PostJob> postJobs = new List<PostJob>();

            foreach (var id in jobIds)
            {
                var job = await _context.PostJobs.Include(j => j.Author).
                    Include(j => j.JobCategory).Include(j => j.SalaryTypes).
                    Include(j => j.ImagePostJobs).ThenInclude(img => img.Image).
                    Where(u => u.PostId == id).Include(j => j.ApplyJobs).Include(j => j.WishJobs).SingleOrDefaultAsync();
                postJobs.Add(job);
            }
            return postJobs;
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
            string sql = "";
            if (s.SortNumberApplied != 0)
            {
                sql = $"SELECT p.* FROM PostJob p left join ApplyJob aj on p.Post_Id = aj.Post_Id WHERE 1=1 and AuthorId = {employerID} ";
            }
            else
            {
                sql = $"SELECT p.* FROM PostJob p WHERE 1=1 and AuthorId = {employerID} ";
            }

            if (!string.IsNullOrEmpty(s.JobKeyWord))
            {
                sql = sql + $" and ( dbo.RemoveDiacritics(p.JobTitle)  LIKE '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}')+'%'";
                sql = sql + $" OR dbo.RemoveDiacritics(JobDescription) like '%'+ dbo.RemoveDiacritics(N'{s.JobKeyWord}%')+'%' ) ";
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
            if (s.IsUrgentRecruitment != -1)
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
                sql = sql + " GROUP BY p.Post_Id, p.JobTitle, p.JobDescription, p.salary_types_id, p.Salary, p.NumberPeople, p.Address, p.latitude, p.longitude, p.AuthorId, p.CreateDate, p.ExpirationDate, p.Status, p.censor_Id, p.censor_Date, p.Reason, p.IsUrgentRecruitment, p.JobCategory_Id, p.time ";
                if (s.SortNumberApplied > 0)
                {
                    sql = sql + " order by COUNT(aj.id), p.CreateDate desc ";
                }
                else
                {
                    sql = sql + " order by COUNT(aj.id) desc, p.CreateDate desc ";
                }
            }
            else
            {
                sql = sql + "order by p.CreateDate desc";
            }
            Console.WriteLine(sql);


            var query = _context.PostJobs.FromSqlRaw(sql);
            var results = await query.ToListAsync();
            var id = results.Select(u => u.PostId);
            return id;
        }


        async Task<IEnumerable<PostJob>> IPostJobRepository.GetAllPostJob(int status)
        {
            if (status == -1)
            {
                return await _context.PostJobs.Include(p => p.Reports).ToListAsync();
            }
            var PostJobs = await _context.PostJobs.Where(p => p.Status == status).Include(p => p.Reports).ToListAsync();
            return PostJobs;
        }

        public async Task<bool> AddWishJob(int jobid, int userid)
        {
            var c = await _context.WishJobs.Where(x => x.JobSeekerId == userid && x.PostJobId == jobid).AnyAsync();
            if (c)
            {
                return false;
            }
            var wj = new WishJob
            {
                PostJobId = jobid,
                JobSeekerId = userid,
            };
            _context.WishJobs.Add(wj);
            var i = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWishJob(int jobid, int userid)
        {
            var c = await _context.WishJobs.Where(x => x.JobSeekerId == userid && x.PostJobId == jobid).SingleOrDefaultAsync();
            if (c == null)
            {
                return false;
            }
            _context.WishJobs.Remove(c);
            var i = await _context.SaveChangesAsync();
            return i >= 1;
        }

        public async Task<IEnumerable<int>> getJobIdInWishList(PostJobSearchWishList s, int userid)
        {
            string sql = $"select p.* from WishJob wj join PostJob p on wj.PostJob_Id = p.Post_Id and wj.JobSeeker_Id = {userid}\r\nleft join ApplyJob aj on aj.Post_Id = p.Post_Id\r\ngroup by p.Post_Id, p.JobTitle, p.JobDescription, p.salary_types_id, p.Salary, p.NumberPeople, p.Address, p.latitude, p.longitude, p.AuthorId, p.CreateDate, p.ExpirationDate, p.Status, p.censor_Id, p.censor_Date, p.Reason, p.IsUrgentRecruitment, p.JobCategory_Id, p.time ";

            if (s.sort == 0)/// uu tien xong viec pho bien
            {
                sql = sql + "order by  COUNT(aj.id) desc";
            }
            else if (s.sort == 1) /// uu tien xong viec luong cao
            {
                sql = sql + "order by p.Salary desc";
            }
            else if (s.sort == 2)/// uu tien xong viec tuyeenr gap
            {
                sql = sql + "order by p.IsUrgentRecruitment desc";
            }
            else if (s.Longitude.HasValue && s.Latitude.HasValue && s.sort == 3) // sort == 3 // uu tien khoangr cach
            {
                sql = sql + $"order by (6371 * ACOS(COS(RADIANS({s.Latitude})) * COS(RADIANS(p.latitude)) * COS(RADIANS(p.longitude) - RADIANS({s.Longitude})) +SIN(RADIANS({s.Latitude})) * SIN(RADIANS(p.latitude)))) asc";
            }
            Console.WriteLine(sql);

            var query = _context.PostJobs.FromSqlRaw(sql);
            var results = await query.ToListAsync();
            var id = results.Select(u => u.PostId);
            return id;

        }

        public async Task<int> ReportJob(Report report)
        {
            var startOfDay = DateTime.Today;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            var reportCount = await _context.Reports
            .Where(r => r.JobSeekerId == report.JobSeekerId && r.CreateDate >= startOfDay && r.CreateDate <= endOfDay)
            .CountAsync();
            if (reportCount >= 3)
            {
                return -1;
            }
            _context.Add(report);
            var i = await _context.SaveChangesAsync();
            return report.ReportId;
        }

        public async Task<bool> CheckJobByIDAndUserid(int id, int userid)
        {
            var c = await _context.PostJobs.Where(pj => pj.PostId == id && pj.AuthorId == userid && (pj.Status == 0|| pj.Status == 3)).AnyAsync();
            return c;
        }

        public async Task<PostJob> GetJobByIDForUpdate(int id)
        {
            var postjob = await _context.PostJobs.Where(pj => pj.PostId == id).Include(pj => pj.ImagePostJobs).SingleOrDefaultAsync();
            return postjob;
        }

        public async Task<int> UpdatePostJob(PostJob postJob)
        {
            var postjobAfter = await _context.PostJobs.Where(pj => pj.PostId == postJob.PostId).SingleOrDefaultAsync();
            if (postjobAfter != null)
            {

                postjobAfter.JobTitle = postJob.JobTitle;
                postjobAfter.JobDescription = postJob.JobDescription;
                postjobAfter.SalaryTypesId = postJob.SalaryTypesId;
                postjobAfter.Salary = postJob.Salary;
                postjobAfter.NumberPeople = postJob.NumberPeople;
                postjobAfter.Address = postJob.Address;
                postjobAfter.Latitude = postJob.Latitude;
                postjobAfter.Longitude = postJob.Longitude;
                postjobAfter.Status = postJob.Status;
                postjobAfter.IsUrgentRecruitment = postJob.IsUrgentRecruitment;
                postjobAfter.JobCategoryId = postJob.JobCategoryId;
                postjobAfter.Time = postJob.Time;

                _context.Entry(postjobAfter).State = EntityState.Modified;
                int i = await _context.SaveChangesAsync();
                return postJob.PostId;
            }
            return 0;
        }

        public async Task<IEnumerable<PostJob>> GetPostJobBuAuthorid(int authorid)
        {

            var postjob = await _context.PostJobs.Where(pj => pj.AuthorId == authorid && pj.Status == 2)
                .Include(j => j.Author).Include(j => j.JobCategory).Include(j => j.SalaryTypes).Include(j => j.ImagePostJobs).ThenInclude(img => img.Image).Include(j => j.ApplyJobs).Include(j => j.WishJobs).ToListAsync();

            return postjob;
        }

        public async Task<IEnumerable<int>> ViewRecommendedJobs(int userid, decimal? userLatitude, decimal? userLongitude)
        {

            var PostAuthor = await _context.PostJobs.Where(pj=>pj.AuthorId == userid).ToListAsync();
            var PostAuthorId = PostAuthor.Select(pj => pj.PostId).ToList();

            string PostAuthorIdStr = "(-1 ";
            foreach(var i in PostAuthorId)
            {
                PostAuthorIdStr = PostAuthorIdStr + ", " + i;
            }
            PostAuthorIdStr = PostAuthorIdStr + " )";

            var query = @"
                    
                    WITH UserCVString AS (
    SELECT 
        cv.UserId,
        STRING_AGG(ic.ItemDescription, ' ') AS CVText
    FROM Cv cv
    INNER JOIN ItemOfCv ic ON ic.CvId = cv.CvId
    WHERE cv.UserId = @UserId
    GROUP BY cv.UserId
),
CVSimilarity AS (
    SELECT 
        pj.Post_Id,
        dbo.CalculateSimilarity(pj.JobDescription, ucv.CVText) AS CVScore
    FROM PostJob pj
    CROSS JOIN UserCVString ucv
    WHERE pj.ExpirationDate >= GETDATE()
),
CategoryRank AS (
    SELECT 
        pj.Post_Id,
        pj.JobCategory_Id,
        COUNT(*) OVER (PARTITION BY pj.JobCategory_Id) AS RankCategory
    FROM ApplyJob aj
    INNER JOIN PostJob pj ON aj.Post_Id = pj.Post_Id
    WHERE aj.Status IN (3, 4) AND aj.JobSeeker_Id = @UserId
),
WishJobMatch AS (
    SELECT 
        pj.Post_Id,
        MAX(dbo.CalculateSimilarity(pj.JobDescription, wj_pj.JobDescription)) AS WishScore
    FROM PostJob pj
    INNER JOIN WishJob wj ON pj.Post_Id = wj.PostJob_Id
    INNER JOIN PostJob wj_pj ON wj.PostJob_Id = wj_pj.Post_Id
    WHERE wj.JobSeeker_Id = @UserId
    GROUP BY pj.Post_Id
),
DistanceCalculation AS (
    SELECT 
        pj.Post_Id,
        pj.latitude,
        pj.longitude,
        CASE 
            WHEN @UserLatitude IS NOT NULL AND @UserLongitude IS NOT NULL THEN
                6371 * ACOS(
                    COS(RADIANS(@UserLatitude)) * COS(RADIANS(pj.latitude)) *
                    COS(RADIANS(pj.longitude) - RADIANS(@UserLongitude)) +
                    SIN(RADIANS(@UserLatitude)) * SIN(RADIANS(pj.latitude))
                )
            ELSE NULL
        END AS DistanceKm
    FROM PostJob pj
    WHERE pj.ExpirationDate >= GETDATE()
)
SELECT top 10 *
FROM (
    SELECT 
        pj.*,
        COALESCE(cr.RankCategory, 0) AS CategoryPriority,
        COALESCE(cs.CVScore, 0) AS CVMatch,
        COALESCE(wm.WishScore, 0) AS WishMatch,
        COALESCE(dc.DistanceKm, 0) AS DistanceKm
    FROM PostJob pj
    LEFT JOIN CategoryRank cr ON pj.Post_Id = cr.Post_Id
    LEFT JOIN CVSimilarity cs ON pj.Post_Id = cs.Post_Id
    LEFT JOIN WishJobMatch wm ON pj.Post_Id = wm.Post_Id
    LEFT JOIN DistanceCalculation dc ON pj.Post_Id = dc.Post_Id
    WHERE pj.ExpirationDate >= GETDATE() AND pj.Status = 2 AND pj.Post_Id NOT IN " + PostAuthorIdStr+@"
) AS OrderedResults
ORDER BY 
    CVMatch DESC,
    CategoryPriority DESC,
    CASE 
        WHEN @UserLatitude IS NOT NULL AND @UserLongitude IS NOT NULL THEN DistanceKm 
        ELSE NULL 
    END ASC, -- Gần nhất lên đầu nếu có tọa độ
    WishMatch DESC;

                    ";
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@UserId", userid),
        new SqlParameter("@UserLatitude", (object?)userLatitude ?? DBNull.Value),
        new SqlParameter("@UserLongitude", (object?)userLongitude ?? DBNull.Value)
    };

            var result = _context.PostJobs.FromSqlRaw(query, parameters.ToArray());
            var results = await result.ToListAsync();
            var id = results.Select(u => u.PostId);
            return id;
        }

        public async Task<PostJob> GetJobByIDForReCreate(int id)
        {
            var postjob = await _context.PostJobs.Where(pj => pj.PostId == id).Include(pj => pj.ImagePostJobs).SingleOrDefaultAsync();
            return postjob;
        }

        public async Task<bool> CheckJobByIDAndUseridForCreate(int id, int userid)
        {
            var c = await _context.PostJobs.Where(pj => pj.PostId == id && pj.AuthorId == userid).AnyAsync();
            return c;
        }
    }
}
