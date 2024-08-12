using System.Security.Cryptography;
using System.Text;

namespace TheGalaxy.Common.Helpers
{
    public static class PasswordHelper
    {
        public static string Generate(int length = 6)
        {
            string valid = "abcdefghijklmnozABCDEFGHIJKLMNOZ1234567890";
            StringBuilder strB = new StringBuilder(100);
            Random random = new Random();
            while (0 < length--)
            {
                strB.Append(valid[random.Next(valid.Length)]);
            }
            return strB.ToString();
        }

        public static string StringToHash(string value, string salt)
        {
            string internalSalt = CreateSalt(salt);
            string saltAndPwd = String.Concat(value, internalSalt);
            UTF8Encoding encoder = new UTF8Encoding();
            using(var sha256hasher = SHA256.Create())
            {
                byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(saltAndPwd));
                string hashedPwd = String.Concat(byteArrayToString(hashedDataBytes), internalSalt);
                return hashedPwd;
            }
        }

        private static string CreateSalt(string salt)
        {
            string internalSalt;
            byte[] userBytes;
            userBytes = Encoding.ASCII.GetBytes(salt);
            long XORED = 0x00;

            foreach (int x in userBytes)
                XORED = XORED ^ x;

            Random rand = new Random(Convert.ToInt32(XORED));
            internalSalt = rand.Next().ToString();
            internalSalt += rand.Next().ToString();
            internalSalt += rand.Next().ToString();
            internalSalt += rand.Next().ToString();
            return internalSalt;
        }

        public static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
    }
}
