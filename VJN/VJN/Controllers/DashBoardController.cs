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
        public async Task<ActionResult<DashBoardDTO>> DashBoard([FromQuery] DashBoardSearchDTO model)
        {
            // tong
            var DashBoardDTO = new DashBoardDTO();
            int TotalUser = await _dashboardService.GetTotalUser();
            UserStatistics UserStatistics = new UserStatistics();
            double EmployersNumber = await _dashboardService.GetTotalEmployer();
            UserStatistics.EmployersNumber = EmployersNumber;
            //------------------------------------------------------------------------------------
            RevenueStatistics RevenueStatistics = await _dashboardService.GetRevenueStatistics(model);
            PackageStatistics PackageStatistics =  await _dashboardService.GetPackageStatistics(model);
            DashBoardDTO.TotalUser = TotalUser;
            DashBoardDTO.UserStatistics = UserStatistics;
            DashBoardDTO.RevenueStatistics = RevenueStatistics;
            DashBoardDTO.PackageStatistics = PackageStatistics;
            return Ok(DashBoardDTO);
        }
    }
}
