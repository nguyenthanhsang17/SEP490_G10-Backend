using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.CvDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CvsController : ControllerBase
    {
        private readonly ICvService _cvService;

        public CvsController(ICvService cvService)
        {
            _cvService = cvService;
        }

        // GET: api/Cvs/5
        [Authorize]
        [HttpGet("GetCvByUID")]
        public async Task<ActionResult<IEnumerable<CvDTODetail>>> GetCvByUID()
        {
            var id_str = GetUserIdFromToken();
            var cv = await _cvService.GetCvByUserID(int.Parse(id_str));
            return Ok(cv);
        }

        [HttpGet("GetAllCV")]
        public async Task<ActionResult<IEnumerable<CvDTODetail>>> GetAllCV()
        {
            var cv = await _cvService.GetCvAllcv();
            return Ok(cv);
        }

        [Authorize]
        [HttpPut("UpdateCv")]
        public async Task<ActionResult<bool>> UpdateCv([FromBody] List<CvUpdateDTO> model)
        {
            var id_str = GetUserIdFromToken();
            var userid = int.Parse(id_str);

            var result = await _cvService.UpdateCV(model, userid);

            return result? Ok(result):BadRequest(result);
        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Missing token in Authorization header.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                throw new Exception("User ID not found in token.");
            }

            return userIdClaim.Value;
        }



    }
}
