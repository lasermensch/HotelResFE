using HotelResFE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            _baseUrl = "https://localhost:44364/api";

        }

        public async Task<string> LoginAsync(LoginCreds creds)
        {
            try
            {
                creds.Password = SecurityService.UnGarble(creds.Password);
                string json = JsonConvert.SerializeObject(creds); //Kanske måste lägga in citationstecken i strängen.

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(new Uri($"{_baseUrl}/auth"), content);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string token = response.Content.ToString();
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return token;
                }

                return null;
            }
            catch (Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
                return null;
            }
        }

        public async Task<HttpStatusCode> RegisterNewUserAsync(User user)
        {
            LoginCreds creds = new LoginCreds() { Email = user.Email, Password = user.Password };

            user.Password = SecurityService.UnGarble(user.Password);
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"{_baseUrl}/users"), content);

            if (response.StatusCode == HttpStatusCode.OK)
                await LoginAsync(creds);

            return response.StatusCode;
        }

        public async Task<User> GetUserAsync()
        {

            try
            {
                var response = await _client.GetAsync(new Uri($"{_baseUrl}/users/userfromtoken"));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    User u = JsonConvert.DeserializeObject<User>(content);
                    return u;
                }
            }
            catch (Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);

            }
            return null;

        }
        public Task<User> EditUser()
        {

            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> DeleteUserAsync()
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.DeleteAsync(new Uri($"{_baseUrl}/users"));
                

            }catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }

            return response.StatusCode;
        }
        public void LogOut()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
            
        }

        

        
    }
}
