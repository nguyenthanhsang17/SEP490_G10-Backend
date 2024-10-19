using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VJN.Authenticate;
using VJN.ModelsDTO.EmailDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private OTPGenerator _generator;
        public EmailController(IEmailService emailService, IUserService userService, OTPGenerator generator)
        {
            _emailService = emailService;
            _userService = userService;
            _generator = generator;
        }

        [HttpPost("SendMailToForgotPassword")]
        public async Task<IActionResult> SendMailToForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            var check = await _userService.CheckEmailExits(email);
            if(check)
            {
                var opt = _generator.GenerateOTP();
                _userService.UpdateOtpUser(email, opt);
                await _emailService.SendEmailAsync(email, "Mã xác nhận OTP cho yêu cầu đặt lại mật khẩu", $"Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Vui lòng sử dụng mã OTP dưới đây để xác thực yêu cầu của bạn:\r\n\r\n**Mã OTP: {opt} \r\n\r\nMã OTP này sẽ hết hạn sau 5 phút. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.");

                return Ok("Mã đã được gửi tới Email của bạn vui lòng kiểm tra !!!");

            }
            else
            {
                return BadRequest("Email không tồn tại");
            }
        }
    }
}
