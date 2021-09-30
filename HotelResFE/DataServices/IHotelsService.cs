using HotelResFE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public interface IHotelsService
    {
        public Task<IEnumerable<Hotel>> GetHotelsAsync();
        public Task<Hotel> GetHotelByIdAsync();
        public Task PostHotelAsync();


    }
}