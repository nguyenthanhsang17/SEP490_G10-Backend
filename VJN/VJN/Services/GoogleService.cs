using Google.Apis.Auth;

namespace VJN.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly string _clientId;

        public GoogleService(IConfiguration configuration)
        {
            _clientId = configuration["Authentication:Google:ClientId"];
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
                return payload;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google Token Validation Failed: {ex.Message}");
                return null;
            }
        }
    }
}
