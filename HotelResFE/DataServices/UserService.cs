using HotelResFE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public UserService(HttpClient httpClient)
        {
            _client = httpClient;
            _baseUrl = "";

        }

        public async Task<string> LoginAsync(LoginCreds creds)
        {
            try
            {
                creds.Password = UnGarble(creds.Password);
                string json = JsonConvert.SerializeObject(creds); //Kanske måste lägga in citationstecken i strängen.

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(new Uri($"{_baseUrl}/auth"), content);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string token = response.Content.ToString();
                    return token;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<HttpStatusCode> RegisterNewUserAsync(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"{_baseUrl}/users"), content);

            return response.StatusCode;
        }

        private string UnGarble(string nonsense)
        {
            byte[] resultArray = null;
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(nonsense);

                keyArray = UTF8Encoding.UTF8.GetBytes("q3t6w9z$C&E)H@Mc");


                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
