using VJN.Models;
using VJN.ModelsDTO.ReportDTOs;

namespace VJN.Services
{
    public interface IRepostService
    {
        public Task<IEnumerable<ReportDTO>> getAllReportByPostId(int postId);
    }
}
