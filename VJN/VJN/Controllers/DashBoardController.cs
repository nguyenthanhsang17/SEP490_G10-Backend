using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VJN.ModelsDTO.DashBoardDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _dashboardService;

        public DashBoardController(IDashBoardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashBoardDTO>> DashBoard()
        {
            // tong
            var DashBoardDTO = new DashBoardDTO();
            int TotalUser = await _dashboardService.GetTotalUser();
            UserStatistics UserStatistics = new UserStatistics();
            double JobSeekersPercentage = await _dashboardService.GetJobSeekersPercentage();
            double EmployersPercentage = await _dashboardService.GetEmployersPercentage();

            UserStatistics.JobSeekersPercentage = JobSeekersPercentage;
            UserStatistics.EmployersPercentage = EmployersPercentage;
            RevenueStatistics RevenueStatistics = await _dashboardService.GetRevenueStatistics();
            PackageStatistics PackageStatistics =  await _dashboardService.GetPackageStatistics();
            DashBoardDTO.TotalUser = TotalUser;
            DashBoardDTO.UserStatistics = UserStatistics;
            DashBoardDTO.RevenueStatistics = RevenueStatistics;
            DashBoardDTO.PackageStatistics = PackageStatistics;
            return Ok(DashBoardDTO);
        }
    }
}
