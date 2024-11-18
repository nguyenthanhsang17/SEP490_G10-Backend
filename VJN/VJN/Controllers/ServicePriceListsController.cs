using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
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
    }
}
