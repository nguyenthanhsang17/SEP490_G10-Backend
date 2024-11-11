using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.ReportDTOs;
using VJN.ModelsDTO.UserDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class ReportService : IRepostService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
             _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<ReportDTO>> getAllReportByPostId(int postId)
        {
            var reports = await _reportRepository.getAllReportById(postId);
            var reportDTOs = reports.Select(r => new ReportDTO
            {
                ReportId = r.ReportId,
                Reason = r.Reason,
                CreateDate = r.CreateDate,
                Status = r.Status,
                JobSeeker = r.JobSeeker != null ? new UserDTOReport
                {
                    UserId = r.JobSeeker.UserId,
                    FullName = r.JobSeeker.FullName,
                    Phonenumber = r.JobSeeker.Phonenumber,
                    Age = r.JobSeeker.Age,
                    Email = r.JobSeeker.Email,
                    AvatarURL = r.JobSeeker.AvatarNavigation.Url 
                } : null,
                ReportMedia = r.ReportMedia 
            }).ToList();
            foreach (var item in reportDTOs)
            {
                foreach (var item1 in item.ReportMedia)
                {
                    item1.Report = null;
                    item1.Image.Users = null;
                    item1.Image.ReportMedia = null;
                }
            }
            return reportDTOs;
            
        }
    }
}
