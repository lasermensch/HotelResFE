using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public static class SecurityService
    {

        public static string Token { get; set; }

        private static string key = "q3t6w9z$C&E)H@Mc";
        public static string Garble(string secret)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(secret);
            Byte[] resultArray = null;
            try
            {
                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tdes.CreateEncryptor();
                    resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string UnGarble(string nonsense)
        {
            byte[] resultArray = null;
            try
            {
                byte[] keyArray;
                byte[] toDecryptArray = Convert.FromBase64String(nonsense);

                keyArray = UTF8Encoding.UTF8.GetBytes("q3t6w9z$C&E)H@Mc");


                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                }


            }
            catch (Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
