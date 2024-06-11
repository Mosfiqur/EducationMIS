using System;
using System.Text;

namespace UnicefEducationMIS.Service.Helpers
{
    internal class PasswordGenerator
    {
        private static int PasswordLength = 8;
        private static bool PasswordNonAlphanumeric = true;
        private static bool RequireDigit = true;
        private static bool RequireLowerCase = false;
        private static bool RequireUpperCase = true;

        public static string GeneratePassword()
        {
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < PasswordLength)
            {
                char c = (char)random.Next(97, 123);

                password.Append(c);

                if (char.IsDigit(c))
                    RequireDigit = false;
                else if (char.IsLower(c))
                    RequireLowerCase = false;
                else if (char.IsUpper(c))
                    RequireUpperCase = false;
                else if (!char.IsLetterOrDigit(c))
                    PasswordNonAlphanumeric = false;
            }

            if (PasswordNonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (RequireDigit)
                password.Append((char)random.Next(48, 58));
            if (RequireLowerCase)
                password.Append((char)random.Next(97, 123));
            if (RequireUpperCase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
    }
}
