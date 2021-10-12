using HotelResFE.Models;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public class HotelsService : IHotelsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        

        public HotelsService(HttpClient client)
        {
            _client = client;
            _baseUrl = "https://localhost:44364/api";
        }
        public async Task<Hotel> GetHotelByIdAsync(string hotelId)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token); //Borde kunna lösas bättre med en eventaggregator.
                var response = await _client.GetAsync($"{_baseUrl}/hotels/{hotelId}");
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    Hotel h = JsonConvert.DeserializeObject<Hotel>(content);
                    return h;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
                var response = await _client.GetAsync($"{_baseUrl}/hotels");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    IEnumerable<Hotel> hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(content);
                    return hotels;
                }   
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public Task PostHotelAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecurityService.Token);
            throw new NotImplementedException();
        }
    }
}
