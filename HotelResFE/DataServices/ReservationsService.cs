using HotelResFE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public class ReservationsService : IReservationsService
    {
        private readonly HttpClient _client;
        private string _baseURL;

        public ReservationsService(HttpClient client)
        {
            _client = client;
            _baseURL = "https://localhost:44364/api";
        }

        public async Task DeleteReservationAsync(Guid reservationId)
        {
            try
            {
                var response = await _client.DeleteAsync(new Uri($"{_baseURL}/reservations/{reservationId}"));
            }
            catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }
        }

        public async Task<Reservation> GetReservationAsync(Guid reservationId)
        {
            try
            {
                var response = await _client.GetAsync(new Uri($"{_baseURL}/reservations/{reservationId}"));
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Reservation r = JsonConvert.DeserializeObject<Reservation>(content);
                    return r;
                }
            }catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }
            return null;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            try
            {
                var response = await _client.GetAsync(new Uri($"{_baseURL}/reservations"));
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reservations = JsonConvert.DeserializeObject<IEnumerable<Reservation>>(content);

                    return reservations;
                }

            }catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(Guid hotelId)
        {
            try
            {
                var response = await _client.GetAsync(new Uri($"{_baseURL}/reservations/by-hotel-id-{hotelId}"));

                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<Reservation>>(content);
                }    
            }catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }
            return null;
        }

        public async Task PostReservationAsync(Reservation reservation)
        {
            try
            {
                var json = JsonConvert.SerializeObject(reservation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(new Uri($"{_baseURL}/reservations"), content);

            }catch(Exception epicFail)
            {
                Debug.WriteLine(epicFail.Message);
            }
        }
    }
}
