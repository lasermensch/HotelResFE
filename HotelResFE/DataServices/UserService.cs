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
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
                var response = await _client.PostAsync(new Uri($"{_baseUrl}/auth"), content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string respCont = await response.Content.ReadAsStringAsync();
                    string token = respCont.Remove(0, 10);
                    token = token.Remove(token.Length - 2, 2);
                    SecurityService.Token = token;
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
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

        public async Task<LoginCreds> RegisterNewUserAsync(User user)
        {
            try
            {
                LoginCreds creds = new LoginCreds() { Email = user.Email, Password = user.Password };

                user.Password = SecurityService.UnGarble(user.Password);
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
                var response = await _client.PostAsync(new Uri($"{_baseUrl}/users"), content);


                return creds;
            }
            catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
                return null;
            }
        }

        public async Task<User> GetUserAsync()
        {

            if (_client.DefaultRequestHeaders.Authorization == null)
                return null;
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
                var response = await _client.GetAsync(new Uri($"{_baseUrl}/users/userfromtoken"));
                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return null;
                Debug.WriteLine(response);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    User u = JsonConvert.DeserializeObject<User>(content);
                    u.Password = SecurityService.Garble(u.Password);
                    return u;
                }
            }
            catch (Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);

            }
            return new();

        }
        public async Task EditUserAsync(User user) //Varför ska vi returnera en user?
        {

            try
            {
                user.Password = SecurityService.UnGarble(user.Password);
                string json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
                var response = await _client.PutAsync(new Uri($"{_baseUrl}/users"), content);

                

            }catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }

           
        }

        public async Task<HttpStatusCode?> DeleteUserAsync()
        {
            
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
                var response = await _client.DeleteAsync(new Uri($"{_baseUrl}/users"));
                return response.StatusCode;

            }
            catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
                return null;
            }

            
        }
        public void LogOut()
        {
            SecurityService.Token = null;
            _client.DefaultRequestHeaders.Authorization = null; //Redundant
            
        }

        

        
    }
}
