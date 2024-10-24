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
using VJN.ModelsDTO.MediaItemDTOs;
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
        private IGoogleService _googleService;
        private readonly IMediaItemService _mediaItemService;

        public UsersController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, IEmailService emailService, OTPGenerator generator, IGoogleService googleService, IMediaItemService mediaItemService)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _generator = generator;
            _googleService = googleService;
            _mediaItemService = mediaItemService;
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

            if (st.Status == 3)
            {
                return Unauthorized(new { Message = "Tài khoản của bạn hiện đang bị khóa" });
            }

            if (st.Status == 0)
            {
                return Unauthorized(new { Message = "Tài khoản của bạn hiện chưa được xác thực." });
            }

            var token = _jwtTokenGenerator.GenerateJwtToken(st);

            return Ok(new
            {
                Message = "Đăng nhập thành công",
                token,
                st.FullName,
                st.RoleId,
                st.Status
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
        [HttpGet("Detail")]
        public async Task<ActionResult<UserDTO>> GetUserdetails()
        {
            string id_str = GetUserIdFromToken();
            int id = int.Parse(id_str);
            Console.WriteLine(id);
            var user = await _userService.findById(id);
            return Ok(user);
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
                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    return BadRequest(new { Message = "mật khẩu mới và mật khẩu xác nhận không giống nhau" });
                }

                var userId = GetUserIdFromToken();

                var result = await _userService.ChangePassword(model.OldPassword, model.NewPassword, model.ConfirmPassword, int.Parse(userId));

                if (result == 0)
                {
                    return BadRequest(new { Message = "Người dùng không tồn tại" });
                }
                if (result == -1)
                {
                    return BadRequest(new { Message = "Nhập sai mật khẩu cũ" });
                }
                return Ok(new { Message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Có lỗi xảy ra: " + ex.Message });
            }

        }

        [HttpPost("VerifycodeForgotPassword")]
        public async Task<IActionResult> VerifyCodeForgotPassword([FromBody] EmailForgotPassword model)
        {
            if (model == null || string.IsNullOrEmpty(model.ToEmail) || string.IsNullOrEmpty(model.Opt)||string.IsNullOrEmpty(model.Password)||string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            var check = await _userService.Verifycode(model.ToEmail, model.Opt);
            if (check)
            {

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    return BadRequest(new { Message = "mật khẩu mới và mật khẩu xác nhận không giống nhau" });
                }
                var user = await _userService.GetUserByEmail(model.ToEmail);
                var c = await _userService.UpdatePassword(user.UserId, model.Password);

                if (c)
                {
                    return Ok(new { Message = "Thay đổi mật khẩu thành công" });
                }
                else
                {
                    return BadRequest(new { Message = "Thay đổi mật khẩu thất bại" });
                }

            }
            else
            {
                return BadRequest(new { Message = "Mã OTP không chính xác" });
            }
        }

        [HttpPost("ResgisterUser")]
        public async Task<IActionResult> ResgisterUser([FromBody] UserCreateDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            var otp = _generator.GenerateOTP();
            var i = await _userService.CreateUser(model, otp);
            if (i == 1)
            {
                return BadRequest(new { Message = "Mật khẩu xác nhận và mật khẩu không giống nhau" });
            }
            if (i == 2)
            {
                return BadRequest(new { Message = "Email Đã đăng ký tài khoản đã có tài khoản" });
            }
            else
            {
                await _emailService.SendEmailAsync(model.Email, "Mã OTP của bạn để hoàn tất đăng ký", $"Cảm ơn bạn đã đăng ký tài khoản tại [Tên công ty/dịch vụ]. Để hoàn tất quá trình xác thực, vui lòng sử dụng mã OTP (One-Time Password) dưới đây:\r\n\r\nMã OTP của bạn: {otp}\r\n\r\nMã OTP này có hiệu lực trong 5 phút.\r\n\r\nNếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này. Để đảm bảo an toàn cho tài khoản của bạn, đừng chia sẻ mã OTP này với bất kỳ ai.\r\n\r\nNếu bạn cần hỗ trợ, đừng ngần ngại liên hệ với chúng tôi tại [email hỗ trợ] hoặc [số điện thoại hỗ trợ].\r\n\r\nCảm ơn bạn!");
                return Ok(new { Message = "Succesfully" });
            }
        }
        [HttpPut("VerifyCodeRegister")]
        public async Task<IActionResult> VerifyCodeRegister([FromBody] EmailRegister model)
        {
            if (model == null || string.IsNullOrEmpty(model.Opt))
            {
                return BadRequest(new { Message = "mã OTP trống" });
            }
            var check = await _userService.VerifycodeRegister(model.Opt);
            if (check)
            {
                return Ok(new { Message = "Xác thực thành công tài khoản" });
            }
            else
            {
                return BadRequest(new { Message = "Mã xác thức không chính xác" });
            }

        }
        [AllowAnonymous]
        [HttpPost("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string token)
        {
            var payload = await _googleService.VerifyGoogleToken(token);
            if (payload == null)
            {
                return Unauthorized("Invalid Google Token");
            }
            var Email = payload.Email;
            var checkEmail = await _userService.CheckEmailExits(Email);
            if (checkEmail)
            {
                var st = await _userService.GetUserByEmail(Email);
                if (st == null)
                {
                    return Unauthorized(new { Message = "Username và password không hợp lệ" });
                }

                if (st.Status == 3)
                {
                    return Unauthorized(new { Message = "Tài khoản của bạn hiện đang bị khóa" });
                }

                if (st.Status == 0)
                {
                    return Unauthorized(new { Message = "Tài khoản của bạn hiện chưa được xác thực." });
                }

                var token_1 = _jwtTokenGenerator.GenerateJwtToken(st);

                return Ok(new
                {
                    Message = "Đăng nhập thành công",
                    token,
                    st.FullName,
                    st.RoleId,
                    st.Status
                });
            }
            else
            {
                var mediaDTo = new MediaItemDTO
                {
                    Url = payload.Picture,
                    Status = true
                };
                var mediaID = await _mediaItemService.CreateMediaItem(mediaDTo);

                var userCreateDTO = new UserCreateDTO
                {
                    Email = Email,
                    FullName = payload.GivenName + " " + payload.FamilyName,
                    Password = "123123",
                    ConfirmPassword = "123123"
                };
                var i = await _userService.CreateUser(userCreateDTO, "");
                if (i == 3)
                {
                    _emailService.SendEmailAsync(Email, "Bạn đã đăng ký Việc Nhanh - Nền tảng tuyển dụng và tìm việc bán thời gian trực tuyến", "Mật khẩu mặc định của bạn là 123123");
                    return Ok(new { Message = "Succesfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Fail" });
                }
            }
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            else
            {
                var userId = GetUserIdFromToken();
                var i = await _userService.UpdateProfile(int.Parse(userId), model);
                if (i)
                {
                    return Ok(new { Message = "Cập nhật Thành công" });
                }
                else
                {
                    return Ok(new { Message = "Cập nhật Thất bại" });
                }

            }
        }
        //ham lay dc userid dua vao token
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
