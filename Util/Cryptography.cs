using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class Cryptography
    {
        public static string Md5Hash(string requireString, HashLength length)
        {
            using (var md5 = MD5.Create()) {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(requireString));
                StringBuilder _ = new StringBuilder();
                for (int i = 0; i < data.Length; i++) {
                    switch (length) {
                        default:
                            _.Append(data[i].ToString("x2"));
                            break;
                        case HashLength.Hash48:
                            _.Append(data[i].ToString("x3"));
                            break;
                        case HashLength.Hash32:
                            _.Append(data[i].ToString("x2"));
                            break;
                        case HashLength.Hash64:
                            _.Append(data[i].ToString("x4"));
                            break;
                    }
                }
                return _.ToString();
            }
        }

        public static string Md5Hash(string requireString)
        {
            return Md5Hash(requireString, HashLength.Hash32);
        }

        public static string RandomString()
        {
            return RandomString(8, RandomFormatter.Default);
        }

        public static string RandomString(int length)
        {
            return RandomString(length, RandomFormatter.Default);
        }

        public static string RandomString(int length, RandomFormatter formatter)
        {
            var chars = string.Empty;

            if ((formatter & RandomFormatter.LowerCasedLetter) == RandomFormatter.LowerCasedLetter)
                chars += Constant.Letters.ToLower();

            if ((formatter & RandomFormatter.UpperCasedLetter) == RandomFormatter.UpperCasedLetter)
                chars += Constant.Letters.ToUpper();

            if ((formatter & RandomFormatter.Number) == RandomFormatter.Number)
                chars += Constant.Numbers;

            var _ = new StringBuilder();
            Random r = new Random(DateTime.Now.ToTimeStamp32());
            for (int i = 0; i < length; i++)
                _.Append(chars.Substring(r.Next(0, chars.Length), 1));
            return _.ToString();
        }

    }

    public enum RandomFormatter
    {
        LowerCasedLetter,
        UpperCasedLetter,
        Number,
        Letter = LowerCasedLetter | UpperCasedLetter,
        Default = LowerCasedLetter | Number
    }

    public enum HashLength
    {
        Hash32 = 1,
        Hash48 = 2,
        Hash64 = 3,
    }
}
