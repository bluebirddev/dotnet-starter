using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bluebird.Core.Starter.Repository
{
    public class Utils
    {
        public static void Copy<T, S>(T src, S dst, string[] except = null)
        {
            var srcProperties = src.GetType().GetProperties();
            var destProperties = dst.GetType().GetProperties();

            foreach (var srcProperty in srcProperties)
            {
                foreach (var dstProperty in destProperties)
                {
                    if ((except == null || except.All(i => i != srcProperty.Name)) && srcProperty.Name == dstProperty.Name && srcProperty.PropertyType == dstProperty.PropertyType)
                    {
                        dstProperty.SetValue(dst, srcProperty.GetValue(src));
                        break;
                    }
                }
            }
        }

        public static bool isValidSouthAfricanId(string IdNumber)
        {
            try
            {
                // check date part
                DateTime.ParseExact(IdNumber.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture);

                // check citizen or other
                if (int.Parse(IdNumber.Substring(10, 1)) != 0 && int.Parse(IdNumber.Substring(10, 1)) != 1)
                    return false;

                var sub = IdNumber.Substring(0, 12);
                var digits = sub.Select(d => int.Parse(d.ToString()));
                var checkSum = digits
                   .Reverse()
                   .Select((d, ix) => {
                       if (ix % 2 == 0)
                       {
                           d *= 2;
                           if (d > 9)
                               d -= 9;
                       }
                       return d;
                   })
                   .Aggregate((memo, d) => (memo += d));
                var controlDigit = checkSum * 9 % 10;

                if (controlDigit < 0)
                    return false;

                if (controlDigit != int.Parse(IdNumber.Substring(12, 1)))
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
