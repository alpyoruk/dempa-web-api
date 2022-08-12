using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CarSalesAPI.Converters
{
    public class Crypto
    {
        public static byte[] Encrypt(string unencrypted)
        {
            if (string.IsNullOrEmpty(unencrypted))
                return null;

            try
            {
                SHA256 sha256 = SHA256Managed.Create();
                byte[] hashValue;
                UTF8Encoding objUtf8 = new UTF8Encoding();
                hashValue = sha256.ComputeHash(objUtf8.GetBytes(unencrypted));
                return hashValue;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}