using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Authenticate;
using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UsersController(IUserService userService, JwtTokenGenerator jwtTokenGenerator)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator; 
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ username và password" });
            }

            var st = await _userService.Login(model.UserName, model.Password);

            if (st == null)
            {
                return Unauthorized(new { Message = "Username và password không hợp lệ" });
            }

            if (st.Status == 0)
            {
                return Unauthorized(new { Message = "Tài khoản của bạn hiện đang bị khóa. Vui lòng liên hệ với quản trị viên để được hỗ trợ." });
            }

            var token = _jwtTokenGenerator.GenerateJwtToken(st);

            return Ok(new
            {
                Message = "Đăng nhập thành công",
                token,
                st.UserId,
                st.UserName,
                st.RoleId
            });
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var userdto = await _userService.getAllUser();
            return Ok(userdto);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var userdto = _userService.findById(id);
            return Ok(userdto);
        }
    }
}
