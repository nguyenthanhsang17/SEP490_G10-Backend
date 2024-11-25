using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ServicePriceListDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicePriceListsController : ControllerBase
    {
        private readonly IServicePriceListService _servicePriceListService;

        public ServicePriceListsController(IServicePriceListService servicePriceListService)
        {
            _servicePriceListService = servicePriceListService;
        }


        // GET: api/ServicePriceLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicePriceListDTO>>> GetServicePriceLists()
        {
            var pr = await _servicePriceListService.GetAllServicePriceListWithStatus1();
            return Ok(pr);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ServicePriceListDTO>>> GetAllServicePriceLists([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var servicePriceLists = await _servicePriceListService.GetAllServicePriceList();
                var pagedResult = servicePriceLists.GetPaged(pageNumber, pageSize);

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching the service price list.", error = ex.Message });
            }
        }

        [HttpGet("GetAllServiecPriceList")]
        public async Task<ActionResult<IEnumerable<ServicePriceListDTO>>> GetAllServicePriceListssss()
        {
            try
            {
                var servicePriceLists = await _servicePriceListService.GetAllServicePriceList();

                return Ok(servicePriceLists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching the service price list.", error = ex.Message });
            }
        }

        [HttpPut("CreateNewService")]
        public async Task<ActionResult<ServicePriceList>> CreateNewService([FromBody] CreateServicePriceListDTO newServicePrice)
        {
            if (newServicePrice == null)
            {
                return BadRequest("Invalid data.");
            }

            var servicePriceList = new ServicePriceList
            {
                ServicePriceName = newServicePrice.ServicePriceName,
                NumberPosts = newServicePrice.NumberPosts,
                NumberPostsUrgentRecruitment = newServicePrice.NumberPostsUrgentRecruitment,
                IsFindJobseekers = newServicePrice.IsFindJobseekers,
                DurationsMonth = newServicePrice.DurationsMonth,
                Price = newServicePrice.Price,
                Status = newServicePrice.Status,
            };
            var createdService = await _servicePriceListService.CreateServicePriceList(servicePriceList);
            return Ok( new { msg = "create successfully" });
        }

        [HttpPut("UpdateStatus")]
        public async Task<ActionResult<IEnumerable<ServicePriceList>>> UpdateStatusPriceLists(int id, int newStatus)
        {
            if (id <= 0 || (newStatus != 0 && newStatus != 1))
            {
                return BadRequest("ID hoặc trạng thái không hợp lệ.");
            }
            var result = await _servicePriceListService.ChangeStatusPriceList(id, newStatus);
            if (!result)
            {
                return NotFound($"Không tìm thấy ServicePriceList với ID = {id}.");
            }

            return Ok($"Cập nhật trạng thái thành công cho ServicePriceList với ID = {id}.");
        }
    }
}
