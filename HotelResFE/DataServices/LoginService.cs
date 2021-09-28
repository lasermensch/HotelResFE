using HotelResFE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public LoginService(HttpClient httpClient)
        {
            _client = httpClient;
            _baseUrl = "";

        }

        public async Task<string> LoginAsync(LoginCreds creds)
        {
            try
            {

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
    }
}
