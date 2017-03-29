using System;
using System.Security.Cryptography;
using System.Text;

namespace CB.CSharp.Extentions
{
    public static class HashCreator
    {
        public static string CreateHash(this string data)
        {
            using (var sha512 = new SHA512Managed())
                return Convert.ToBase64String(sha512.ComputeHash(Encoding.ASCII.GetBytes(data)));
        }
    }
}