using VJN.Models;

namespace VJN.Repositories
{
    public class ReportMediaRepository : IReportMediaRepository
    {
        public readonly VJNDBContext _context;

        public ReportMediaRepository(VJNDBContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateReportMedia(int reportid, List<int> images)
        {
            foreach (var image in images)
            {
                var ReportMedia = new ReportMedium();
                ReportMedia.ReportId = reportid;
                ReportMedia.ImageId = image;
                _context.ReportMedia.Add(ReportMedia);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
