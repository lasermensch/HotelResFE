using HotelResFE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public class HotelsService : IHotelsService
    {
        private readonly HttpClient _client;
        public HotelsService(HttpClient client)
        {
            _client = client;
        }
        public async Task<Hotel> GetHotelByIdAsync(string hotelId)
        {
            try
            {
                var response = await _client.GetAsync($"https://localhost:44364/api/hotels/{hotelId}");
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
                var response = await _client.GetAsync("https://localhost:44364/api/hotels");
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
            throw new NotImplementedException();
        }
    }
}
