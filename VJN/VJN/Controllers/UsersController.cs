﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using VJN.Authenticate;
using VJN.Models;
using VJN.ModelsDTO.EmailDTOs;
using VJN.ModelsDTO.UserDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;
        private OTPGenerator _generator;

        public UsersController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, IEmailService emailService, OTPGenerator generator)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _generator = generator;
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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var userdto = await _userService.getAllUser();
            return Ok(userdto);
        }

        // GET: api/Users/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var userdto = _userService.findById(id);
            return Ok(userdto);
        }
        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.ConfirmPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.OldPassword))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            if (model == null)
            {
                return BadRequest(new { Message = "null object" });
            }
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                var result = await _userService.ChangePassword(model.OldPassword, model.NewPassword, model.ConfirmPassword, int.Parse(userId));

                if (result == 0)
                {
                    return BadRequest(new { Message = "Người dùng không tồn tại" });
                }
                if (result == -1)
                {
                    return BadRequest(new { Message = "Nhập sai mật khẩu cũ" });
                }
                if (result == -2)
                {
                    return BadRequest(new { Message = "mật khẩu mới và mật khẩu xác nhận không giống nhau" });
                }
                return Ok(new { Message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Có lỗi xảy ra: " + ex.Message });
            }

        }

        [HttpGet("VerifycodeForgotPassword")]
        public async Task<IActionResult> VerifyCodeForgotPassword([FromBody] EmailForgotPassword model)
        {
            if (model == null || string.IsNullOrEmpty(model.ToEmail) || string.IsNullOrEmpty(model.Opt))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            if (model == null)
            {
                return BadRequest(new { Message = "null object" });
            }
            var check  = await _userService.Verifycode(model.ToEmail, model.Opt);
            if (check)
            {
                return Ok(new { Message = "Verify Successfuly" });
            }
            else
            {
                return BadRequest(new { Message = "Mã OTP không chính xác" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResgisterUser([FromBody] UserCreateDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.UserName)||string.IsNullOrEmpty(model.FullName)||string.IsNullOrEmpty(model.Password)||string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            var i = await _userService.CreateUser(model);
            if (i == 0)
            {
                return BadRequest(new { Message = "null object" });
            }
            if(i==1)
            {
                return BadRequest(new { Message = "Mật khẩu xác nhận và mật khẩu không giống nhau"});
            }
            if (i == 2)
            {
                return BadRequest(new { Message = "Email Đã đăng ký tài khoản có tài khoản"});
            }
            else
            {
                var otp = _generator.GenerateOTP();
                await _emailService.SendEmailAsync(model.Email, "Mã OTP của bạn để hoàn tất đăng ký", $"Cảm ơn bạn đã đăng ký tài khoản tại [Tên công ty/dịch vụ]. Để hoàn tất quá trình xác thực, vui lòng sử dụng mã OTP (One-Time Password) dưới đây:\r\n\r\nMã OTP của bạn: {otp}\r\n\r\nMã OTP này có hiệu lực trong 5 phút.\r\n\r\nNếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này. Để đảm bảo an toàn cho tài khoản của bạn, đừng chia sẻ mã OTP này với bất kỳ ai.\r\n\r\nNếu bạn cần hỗ trợ, đừng ngần ngại liên hệ với chúng tôi tại [email hỗ trợ] hoặc [số điện thoại hỗ trợ].\r\n\r\nCảm ơn bạn!");
                return Ok(new { Message = "Succesfully" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> VerifyCodeRegister([FromBody] EmailRegister model)
        {
            if(model == null || string.IsNullOrEmpty(model.ToEmail)||string.IsNullOrEmpty(model.Opt))
            {
                return BadRequest(new { Message = "mã OTP trống" });
            }
            var check = await _userService.Verifycode(model.ToEmail, model.Opt);
            if (check)
            {
                await _userService.UpdateStatusByEmail(model.ToEmail, 1);
                return Ok(new { Message = "" });
            }
            else
            {
                return BadRequest(new { Message = "Mã xác thức không chính xác" });
            }

        }
    }
}
