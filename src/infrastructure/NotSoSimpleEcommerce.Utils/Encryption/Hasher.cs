using System.Security.Cryptography;
using System.Text;

namespace NotSoSimpleEcommerce.Utils.Encryption
{
    public static class Hasher
    {
        public static string GetMd5(byte[] data)
        {
            using var hash = MD5.Create();
            var md5 = hash.ComputeHash(data);
            return ParseToString(md5);
        }

        private static string ParseToString(byte[] md5)
        {
            var stringBuilder = new StringBuilder();
            foreach (var @byte in md5)
                stringBuilder.Append(@byte.ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
