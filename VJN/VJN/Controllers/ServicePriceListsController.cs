﻿using System;
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
        public async Task<ActionResult<IEnumerable<ServicePriceList>>> GetServicePriceLists()
        {
            var pr = await _servicePriceListService.GetAllServicePriceList();
            return Ok(pr);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ServicePriceList>>> GetAllServicePriceLists([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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

        [HttpPut("CreateNewService")]
        public async Task<ActionResult<ServicePriceList>> CreateNewService([FromBody] CreateServicePriceListDTO newServicePrice)
        {
            if (newServicePrice == null)
            {
                return BadRequest("Invalid data.");
            }

            var servicePriceList = new ServicePriceList
            {
                NumberPosts = newServicePrice.NumberPosts,
                NumberPostsUrgentRecruitment = newServicePrice.NumberPostsUrgentRecruitment,
                IsFindJobseekers = newServicePrice.IsFindJobseekers,
                DurationsMonth = newServicePrice.DurationsMonth,
                Price = newServicePrice.Price
            };
            var createdService = await _servicePriceListService.CreateServicePriceList(servicePriceList);
            return Ok( new { msg = "create successfully" });
        }
    }
}
