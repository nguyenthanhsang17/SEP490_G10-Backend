
using VJN.Repositories;

namespace VJN.Services
{
    public class ReportMediaServices : IReportMediaServices
    {
        private readonly IReportMediaRepository _reportMediaRepository;
        public ReportMediaServices(IReportMediaRepository reportMediaRepository)
        {
            _reportMediaRepository = reportMediaRepository;
        }

        public async Task<bool> CreateReportMedia(int reportid, List<int> images)
        {
            var c = await _reportMediaRepository.CreateReportMedia(reportid, images);
            return c;
        }
    }
}
