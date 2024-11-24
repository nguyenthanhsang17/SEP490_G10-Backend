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
        [HttpPost("SendMailToTest")]
        public async Task<IActionResult> SendMailToTest()
        {
            string htmlContent = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Eco Green Email Template</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;600&display=swap');

        body {
            font-family: 'Montserrat', Arial, sans-serif;
            line-height: 1.6;
            background-color: #e6f3e6;
            margin: 0;
            padding: 0;
            color: #2c3e50;
        }

        .email-container {
            max-width: 600px;
            margin: 30px auto;
            background-color: white;
            border-radius: 15px;
            box-shadow: 0 12px 30px rgba(0,0,0,0.08);
            overflow: hidden;
            border-top: 6px solid #2ecc71;
        }

        .email-header {
            background: linear-gradient(135deg, #2ecc71, #27ae60);
            color: white;
            text-align: center;
            padding: 25px;
            position: relative;
        }

        .email-header h1 {
            margin: 0;
            font-weight: 600;
            font-size: 26px;
            letter-spacing: 1px;
            text-shadow: 1px 1px 2px rgba(0,0,0,0.2);
        }

        .email-body {
            padding: 30px;
            background-color: #f9fff9;
        }

        .email-body h2 {
            color: #27ae60;
            margin-bottom: 15px;
            font-weight: 600;
            border-bottom: 2px solid #2ecc71;
            padding-bottom: 10px;
        }

        .email-body p {
            color: #34495e;
            margin-bottom: 20px;
        }

        .btn {
            display: inline-block;
            background: linear-gradient(135deg, #2ecc71, #27ae60);
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 30px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
            transition: all 0.3s ease;
            box-shadow: 0 5px 15px rgba(46,204,113,0.3);
        }

        .btn:hover {
            transform: translateY(-3px);
            box-shadow: 0 7px 20px rgba(46,204,113,0.4);
            background: linear-gradient(135deg, #27ae60, #2ecc71);
        }

        .feature-list {
            background-color: #f0f9f0;
            padding: 20px;
            border-radius: 10px;
            margin-top: 20px;
        }

        .feature-list ul {
            list-style-type: none;
            padding: 0;
        }

        .feature-list ul li {
            padding: 8px 0;
            border-bottom: 1px solid #dff0df;
            color: #2c3e50;
        }

        .feature-list ul li:last-child {
            border-bottom: none;
        }

        .email-footer {
            background-color: #e6f3e6;
            text-align: center;
            padding: 15px;
            font-size: 12px;
            color: #7f8c8d;
        }

        .social-links {
            margin-top: 20px;
            display: flex;
            justify-content: center;
            gap: 20px;
        }

        .social-links a {
            color: #2ecc71;
            text-decoration: none;
            font-size: 16px;
            transition: color 0.3s ease;
        }

        .social-links a:hover {
            color: #27ae60;
        }

        @media only screen and (max-width: 600px) {
            .email-container {
                width: 100%;
                margin: 0;
                border-radius: 0;
            }
        }
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""email-header"">
            <h1>Chào Mừng Bạn Đến QuickJob</h1>
        </div>
        
        <div class=""email-body"">
            <h2>Xin Chào Quý Khách,</h2>
            
            <p>Chúc mừng bạn đã đăng ký thành công dịch vụ của chúng tôi. Chúng tôi rất vui mừng được phục vụ bạn.</p>
            
            <div class=""feature-list"">
                <ul>
                    <li>Trải nghiệm dịch vụ chuyên nghiệp</li>
                    <li>✓ Hỗ trợ 24/7</li>
                    <li>✓ Cam kết chất lượng</li>
                </ul>
            </div>
        </div>
        
        <div class=""email-footer"">
            <p>&copy; 2023 Tên Công Ty. Mọi quyền được bảo lưu.</p>
        </div>
    </div>
</body>
</html>";
            await _emailService.SendEmailAsyncWithHTML("tuandahe163847@fpt.edu.vn", "Test", htmlContent);
            return Ok(true);
        }



        [HttpPost("SendMailToForgotPassword")]
        public async Task<IActionResult> SendMailToForgotPassword([FromBody] SendMailToForgotPassword model)
        {
            if (string.IsNullOrEmpty(model.email))
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            var check = await _userService.CheckEmailExits(model.email);
            if (check)
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
