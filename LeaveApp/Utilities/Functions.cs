using System.Text;
using System;

namespace LeaveApp.Utilities
{
    public class Functions
    {
        public static string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            // Append random characters to the password until it reaches the specified length
            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(validChars.Length);
                sb.Append(validChars[index]);
            }

            return sb.ToString();
        }
    }
}
