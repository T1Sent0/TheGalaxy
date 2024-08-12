using System.Security.Cryptography;
using System.Text;

namespace TheGalaxy.Common
{
    public static class StringHelper
    {
        private const string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GetRandomString(int length)
        {
            var result = new char[length];
            int alphanumericLength = alphanumeric.Length;

            for (int i = 0; i < length; i++)
            {
                // Генерация случайного индекса и получение соответствующего символа
                int randomIndex = RandomNumberGenerator.GetInt32(alphanumericLength);
                result[i] = alphanumeric[randomIndex];
            }

            return new string(result);
        }

        public static string Sha256Representation(string value)
        {
            var stringBuilder = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                //Getting hashed byte array
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
                foreach (var b in result)
                    stringBuilder.Append(b.ToString("x2")); //Byte as hexadecimal format
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Setting DateTime in JS-readable format (UNIX)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="isUTC"></param>
        /// <returns></returns>
        public static string DateInJSFormat(System.DateTime date, bool isUTC = true)
        {
            var kind = isUTC ? DateTimeKind.Utc : DateTimeKind.Local;
            var milliseconds = date.Subtract(new System.DateTime(1970, 1, 1, 0, 0, 0, kind)).TotalMilliseconds;
            return Math.Round(milliseconds).ToString();
        }
    }
}
