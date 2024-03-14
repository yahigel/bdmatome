using System;
using System.Security.Cryptography;

namespace bdaAPI.Common
{
    public class SessionIdGenerator
    {
        public static string GenerateSessionId()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[32]; // 256ビット
                randomNumberGenerator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes); // バイト配列をBase64文字列に変換
            }
        }
    }
}