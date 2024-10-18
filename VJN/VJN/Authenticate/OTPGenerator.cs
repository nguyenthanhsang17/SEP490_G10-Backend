using System.Security.Cryptography;

namespace VJN.Authenticate
{
    public class OTPGenerator
    {
        public string GenerateOTP(int length = 5)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);
                var otp = BitConverter.ToUInt32(bytes, 0) % (uint)Math.Pow(10, length);
                return otp.ToString().PadLeft(length, '0');
            }
        }
    }
}
