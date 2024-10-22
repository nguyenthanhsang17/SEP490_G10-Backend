using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IActionResult> SendMailToForgotPassword([FromBody] SendMailToForgotPassword model)
        {
            if (string.IsNullOrEmpty(model.email))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            var check = await _userService.CheckEmailExits(model.email);
            if(check)
            {
                var opt = "";
                do
                {
                    opt = _generator.GenerateOTP();
                    bool c = await _userService.CheckOtpExits(opt);
                    if (c == false)
                    {
                        break;
                    }
                } while (true);
                _userService.UpdateOtpUser(model.email, opt);
                await _emailService.SendEmailAsync(model.email, "Mã xác nhận OTP cho yêu cầu đặt lại mật khẩu", $"Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Vui lòng sử dụng mã OTP dưới đây để xác thực yêu cầu của bạn:\r\n\r\n**Mã OTP: {opt} \r\n\r\nMã OTP này sẽ hết hạn sau 5 phút. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.");

                return Ok(new { Message = "Mã đã được gửi tới Email của bạn vui lòng kiểm tra !!!" });

            }
            else
            {
                return BadRequest(new { Message = "Mã đã được gửi tới Email của bạn vui lòng kiểm tra !!!" });
            }
        }
    }
}
