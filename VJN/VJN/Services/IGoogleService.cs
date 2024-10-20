using Google.Apis.Auth;

namespace VJN.Services
{
    public interface IGoogleService
    {
        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token);
    }
}
