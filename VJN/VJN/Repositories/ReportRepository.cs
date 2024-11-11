using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly VJNDBContext _context;
        public ReportRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> getAllReportById(int postId)
        {
            return _context.Reports.Where(r=>r.PostId == postId)
                .Include(r=>r.ReportMedia).ThenInclude(rm=>rm.Image)
                .Include(p => p.JobSeeker).ThenInclude(j => j.AvatarNavigation)
                .ToList();
        }
    }
}
