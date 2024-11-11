using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {


        private readonly IRepostService _reportService;

        public ReportController(IRepostService reportService)
        {
            _reportService=reportService;
        }

        [HttpGet("GetAllReportByPostId")]
        public async Task<IActionResult> GetAllReportByPostID(int postID, int pageNumber = 1, int pageSize = 10, string sortOrder = "asc")
        {
            var reportDTOs = await _reportService.getAllReportByPostId(postID);

            // Sắp xếp các báo cáo dựa trên `CreateDate`
            var sortedReports = sortOrder.ToLower() == "desc"
                ? reportDTOs.OrderByDescending(r => r.CreateDate)
                : reportDTOs.OrderBy(r => r.CreateDate);

            // Áp dụng phân trang
            var pagedReports = sortedReports.GetPaged(pageNumber, pageSize);

            return Ok(pagedReports);
        }


    }
}
