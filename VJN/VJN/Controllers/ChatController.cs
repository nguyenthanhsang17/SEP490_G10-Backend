using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Text;
using VJN.ModelsDTO.ChatDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [Authorize]
        [HttpGet("GetChatUsers")]
        public async Task<ActionResult< IEnumerable<ChatListDTO>>> GetChatUsers()
        {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            var result = await _chatService.GetChatUsers(userid);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetAllChat/{id}")]
        public async Task<ActionResult<IEnumerable<ChatListDTO>>> GetAllChat(int id)
        {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            var result = await _chatService.GetAllChat(userid, id);
            return Ok(result);
        }

        [HttpGet("/ws/chat")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                // Lắng nghe kết nối WebSocket
                await HandleWebSocketConnectionAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private static List<WebSocket> _clients = new List<WebSocket>();

        private static async Task HandleWebSocketConnectionAsync(WebSocket webSocket)
        {
            if (!_clients.Contains(webSocket))
            {
                _clients.Add(webSocket);
            }

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var clientMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Received: " + clientMessage);

                // Gửi thông điệp đến các client khác
                Console.WriteLine($"Current connected clients: {_clients.Count}");

                foreach (var client in _clients)
                {
                    if (client.State == WebSocketState.Open && client != webSocket)
                    {
                        await client.SendAsync(
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes(clientMessage)),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                        Console.WriteLine("gưi thanh cong");
                    }
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // Loại bỏ kết nối khi client đóng
            _clients.Remove(webSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }


        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            if (string.IsNullOrEmpty(token))
            {
                return null;
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
                return null;
            }

            return userIdClaim.Value;
        }
    }
}
