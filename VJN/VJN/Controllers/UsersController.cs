using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using VJN.Authenticate;
using VJN.Models;
using VJN.ModelsDTO.EmailDTOs;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.ModelsDTO.RegisterEmployer;
using VJN.ModelsDTO.UserDTOs;
using VJN.Paging;
using VJN.Services;
using static System.Net.WebRequestMethods;

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
        private readonly ImagekitClient _imagekitClient;
        private readonly IRegisterEmployerMediaService _registerEmployerMediaService;
        private readonly IRegisterEmployerService _registerEmployerService;

        public UsersController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, IEmailService emailService, OTPGenerator generator, IGoogleService googleService, IMediaItemService mediaItemService, IRegisterEmployerMediaService registerEmployerMediaService, IRegisterEmployerService registerEmployerService)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _generator = generator;
            _googleService = googleService;
            _mediaItemService = mediaItemService;
            _imagekitClient = new ImagekitClient("public_Q+yi7A0O9A+joyXIoqM4TpVqOrQ=", "private_e2V3fNLKwK0pGwSrEmFH+iKQtks=", "https://ik.imagekit.io/ryf3sqxfn");
            _registerEmployerMediaService = registerEmployerMediaService;
            _registerEmployerService = registerEmployerService;
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
                var otp = _generator.GenerateOTP();
                _userService.InsertOTP(st.UserId, otp);

                string html = _emailService.GetEmailHTML("Bạn đã quay trở lại QuickJob", $"Mã OTP của bạn để hoàn tất đăng ký", $"Để hoàn tất quá trình xác thực, vui lòng sử dụng mã OTP (One-Time Password) dưới đây:\\r\\n\\r\\nMã OTP của bạn: {otp}\\r\\n\\r\\nMã OTP này có hiệu lực trong 5 phút.\\r\\n\\r\\nNếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này. Để đảm bảo an toàn cho tài khoản của bạn, đừng chia sẻ mã OTP này với bất kỳ ai.\\r\\n\\r\\nNếu bạn cần hỗ trợ, đừng ngần ngại liên hệ với chúng tôi tại [email hỗ trợ] hoặc [số điện thoại hỗ trợ].\\r\\n\\r\\nCảm ơn bạn!\"");
                await _emailService.SendEmailAsyncWithHTML(st.Email, "Mã OTP của bạn để hoàn tất đăng ký", html);
                return Unauthorized(new { Message = "Tài khoản của bạn hiện chưa được xác thực." });
            }
            bool haveProfile = false;
            if (st.Age.HasValue)
            {
                haveProfile = true;
            }
            var token = _jwtTokenGenerator.GenerateJwtToken(st);
            return Ok(new
            {
                Message = "Đăng nhập thành công",
                token,
                st.UserId,
                st.FullName,
                st.RoleId,
                st.Status,
                st.AvatarURL,
                haveProfile,
            });
        }

        // GET: api/Users

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var userdto = await _userService.getAllUser();
            return Ok(userdto);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetAllUsers([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10,[FromQuery] string? name = null,[FromQuery] int? role = null,[FromQuery] int? status = null)
        {
            // Lấy danh sách tất cả người dùng từ service
            var users = await _userService.getAllUser();

            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(u => u.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (role.HasValue)
            {
                users = users.Where(u => u.RoleId == role.Value).ToList();
            }

            if (status.HasValue)
            {
                users = users.Where(u => u.Status == status.Value).ToList();
            }

            var pagedResult = users.GetPaged(pageNumber, pageSize);

            return Ok(pagedResult);
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

        [HttpGet("GetUserDetail")]
        public async Task<ActionResult<UserDTO>> ViewUserdetail(int id)
        {
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
            if (model == null || string.IsNullOrEmpty(model.ToEmail) || string.IsNullOrEmpty(model.Opt) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword))
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
                string html = _emailService.GetEmailHTML("QuickJob", "Mã OTP của bạn để hoàn tất đăng ký", $"Cảm ơn bạn đã đăng ký tài khoản tại VJN. Để hoàn tất quá trình xác thực, vui lòng sử dụng mã OTP (One-Time Password) dưới đây: Mã OTP của bạn: {otp} Mã OTP này có hiệu lực trong 5 phút.Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này. Để đảm bảo an toàn cho tài khoản của bạn, đừng chia sẻ mã OTP này với bất kỳ ai.Nếu bạn cần hỗ trợ, đừng ngần ngại liên hệ với chúng tôi tại [email hỗ trợ] hoặc [số điện thoại hỗ trợ].Cảm ơn bạn!");

                await _emailService.SendEmailAsync(model.Email, "Mã OTP của bạn để hoàn tất đăng ký", html);
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
        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UserUpdateDTO model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            else
            {
                var userId = GetUserIdFromToken();
                var avatarID = 0;
                if (model.AvatarURL == null || model.AvatarURL.Length == 0)
                {
                    Console.WriteLine("User ko cap nhat avatar");
                }
                else
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        await model.AvatarURL.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        Console.WriteLine("day la file name: " + model.AvatarURL.FileName);
                        try
                        {
                            FileCreateRequest uploadRequest = new FileCreateRequest
                            {
                                file = fileBytes,
                                fileName = model.AvatarURL.FileName,
                                overwriteFile = true
                            };
                            Result result = _imagekitClient.Upload(uploadRequest);
                            var media = new MediaItemDTO
                            {
                                Url = result.url,
                                Status = true
                            };
                            Console.WriteLine(Url);
                            avatarID = await _mediaItemService.CreateMediaItem(media);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                Console.WriteLine("day la mediaID: " + avatarID);
                var i = await _userService.UpdateProfile(int.Parse(userId), model, avatarID);
                if (i)
                {
                    return Ok(new { Message = "Cập nhật Thành công" });
                }
                else
                {
                    return BadRequest(new { Message = "Cập nhật Thất bại" });
                }

            }
        }
        [Authorize]
        [HttpPost("VerifyEmployerAccount")]
        public async Task<IActionResult> VerifyEmployerAccount([FromForm] VerifyEmployerAccountDTO dto)
        {
            Console.WriteLine(dto.BussinessName);
            Console.WriteLine(dto.BussinessAddress);
            var id_str = GetUserIdFromToken();
            var id = int.Parse(id_str);
            Console.WriteLine(id);
            int Registerid = await _registerEmployerService.RegisterEmployer(dto, id);
            Console.WriteLine($"Register {id_str}");
            if (Registerid == -1)
            {
                return BadRequest("Đã đăng ký để trở thành nhà tuyển dụng, đợi duyệt");
            }
            if (Registerid == -2)
            {
                return BadRequest("Đã đăng ký để trở thành nhà tuyển dụng");
            }

            if (dto == null)
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            if (dto.files == null || !dto.files.Any())
                return BadRequest("No files provided.");

            var mediaIds = new List<int>();

            var uploadTasks = dto.files.Select(async file =>
            {
                if (file.Length == 0)
                    throw new ArgumentException("One or more files are empty.");

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();

                    FileCreateRequest uploadRequest = new FileCreateRequest
                    {
                        file = fileBytes,
                        fileName = file.FileName
                    };

                    Result result = _imagekitClient.Upload(uploadRequest);
                    var media = new MediaItemDTO
                    {
                        Url = result.url,
                        Status = true
                    };
                    return await _mediaItemService.CreateMediaItem(media);
                }
            });

            try
            {
                mediaIds = (await Task.WhenAll(uploadTasks)).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Upload failed: {ex.Message}");
            }

            var result = await _registerEmployerMediaService.CreateRegisterEmployerMedia(Registerid, mediaIds);
            return Ok(result);
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
            /// check tên claim
            //foreach (var claim in jwtToken.Claims)
            //{
            //    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            //}
            /// check tên claim
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                throw new Exception("User ID not found in token.");
            }

            return userIdClaim.Value;
        }

        [HttpGet("ViewRegisterEmployerList")]
        public async Task<PagedResult<RegisterEmployerDTOforDetail>> ViewRegisterEmployerList(int status,string? searchFullName = null,string? sortOrder = "asc",int pageNumber = 1,int pageSize = 10)
        {

            var res = await _registerEmployerService.getRegisterEmployerByStatus(status);


            if (!string.IsNullOrEmpty(searchFullName))
            {
                res = res.Where(re => re.User != null && re.User.FullName.Contains(searchFullName, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            res = sortOrder?.ToLower() == "desc"
                ? res.OrderByDescending(re => re.CreateDate).ToList()
                : res.OrderBy(re => re.CreateDate).ToList();


            var result = res.Select(re => new RegisterEmployerDTOforDetail
            {
                RegisterEmployerId = re.RegisterEmployerId,
                UserId = re.UserId,
                BussinessName = re.BussinessName,
                BussinessAddress = re.BussinessAddress,
                CreateDate = re.CreateDate,
                Status = re.Status,
                User = re.User != null ? new UserDTOinRegisterEmployer
                {
                    UserId = re.User.UserId,
                    Email = re.User.Email,
                    AvatarURL = re.User.AvatarNavigation?.Url,
                    FullName = re.User.FullName,
                    Age = re.User.Age,
                    Phonenumber = re.User.Phonenumber,
                    Gender = re.User.Gender,
                } : null,
                ListIMG = re.RegisterEmployerMedia?.Select(media => media.Media.Url).ToList() ?? new List<string>()
            });


            var pagedResult = result.GetPaged(pageNumber, pageSize);

            return pagedResult;
        }

        [HttpGet("RegisterEmployerDetail")]
        public async Task<ActionResult<RegisterEmployerDTOforDetail>> RegisterEmployerDetail(int id)
        {
            var re = await _registerEmployerService.getRegisterEmployerByID(id);

            if (re == null)
            {
                return NotFound();
            }

            var result = new RegisterEmployerDTOforDetail
            {
                RegisterEmployerId = re.RegisterEmployerId,
                UserId = re.UserId,
                BussinessName = re.BussinessName,
                BussinessAddress = re.BussinessAddress,
                CreateDate = re.CreateDate,
                Status= re.Status,
                reason=re.Reason,
                User = re.User != null ? new UserDTOinRegisterEmployer
                {
                    UserId = re.User.UserId,
                    Email = re.User.Email,
                    AvatarURL = re.User.AvatarNavigation.Url,
                    FullName = re.User.FullName,
                    Age = re.User.Age,
                    Phonenumber = re.User.Phonenumber,
                    Gender = re.User.Gender,
                } : null,
                ListIMG = re.RegisterEmployerMedia?.Select(media => media.Media.Url).ToList() ?? new List<string>(),
            };

            return Ok(result);
        }

        [HttpPost("Accept/{id}")]
        public async Task<IActionResult> AcceptEmployer(int id)
        {
            var result = await _registerEmployerService.AcceptRegisterEmployer(id);
            if (result)
            {
                var rg = await _registerEmployerService.getRegisterEmployerByID(id);
                var user = await _userService.findById((int)rg.UserId);
                string html = _emailService.GetEmailHTML("Chúc mừng bạn đã trở thành nhà tuyển dụng ", $"Đơn đăng ký trở thành nhà tuyển dụng của bạn đã được xác nhận ", $"Rất nhiều ứng viên đang chờ ban . Hãy bắt đầu tạo một công việc mới ngay thôi ");
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Bạn đã trở thành nhà tuyển dụng", html);
                return Ok(new { message = "Đã chấp thuận nhà tuyển dụng." });
            }
            return BadRequest(new { message = "Không tìm thấy nhà tuyển dụng hoặc có lỗi xảy ra." });
        }


        [HttpPost("Reject/{id}")]
        public async Task<IActionResult> RejectEmployer(int id, [FromBody] string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                return BadRequest(new { message = "Vui lòng nhập lý do từ chối." });
            }

            var result = await _registerEmployerService.RejectRegisterEmployer(id, reason);
            if (result)
            {
                var rg = await _registerEmployerService.getRegisterEmployerByID(id);
                var user = await _userService.findById((int)rg.UserId);
                string html = _emailService.GetEmailHTML("Bạn đã bị từ chối trở thânhf nahf tuyển dụng", $"Đơn đăng ký trở thành nhà tuyển dụng của bạn đã bị từ chối ", $" Lý do từ chối : {reason}");
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Yêu cầu trở thành nhà tuyển dụng của bạn đã bị từ chối ", html);
                return Ok(new { message = "Đã từ chối nhà tuyển dụng." });
            }
            return BadRequest(new { message = "Không tìm thấy nhà tuyển dụng hoặc có lỗi xảy ra." });
        }

        [HttpPost("Ban_Unban_user/{id}")]
        public async Task<IActionResult> ban_unban(int id, [FromBody] string reason, bool ban)
        {
            if (ban && string.IsNullOrEmpty(reason))
            {
                return BadRequest(new { message = "Vui lòng nhập lý do cấm." });
            }
            string msg = "Đã cấm người dùng.";
            if (!ban)
            {
                msg = "đã gỡ cấm người dùng ";

                var user = await _userService.findById(id);
                string html = _emailService.GetEmailHTML("Tài khoản của bạn đã được gỡ cấm ", $"Lưu ý chấp hành nghiêm chỉnh các quy tắc của chúng tôi ", $" Chúc bạn ngày mới tốt lạnh");
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Tài khoản của bạn đã được gỡ cấm", html);
            }
            else {

                var user = await _userService.findById(id);
                string html = _emailService.GetEmailHTML("Tài khoản của bạn đã bị gỡ cấm ", $"Lý do cấm {reason} ", $" Nếu có bất kỳ thắc mắc nào hãy liên hệ với chúng tôi ");
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Tài khoản của bạn đã Bị cấm ", html);

            }
            var result = await _userService.Ban_Unbanuser(id,ban);
            if (result==1)
            {
                return Ok(new { message = msg });
            }
            return BadRequest(new { message = "Không tìm thấy người dùng hoặc có lỗi xảy ra." });
        }

    }
}
