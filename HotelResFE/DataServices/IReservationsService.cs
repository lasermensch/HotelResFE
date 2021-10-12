using HotelResFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.DataServices
{
    public interface IReservationsService
    {
        public Task<IEnumerable<Reservation>> GetReservationsAsync();
        public Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(Guid hotelId);
        public Task<Reservation> GetReservationAsync(Guid reservationId);

        public Task PostReservationAsync();
        public Task DeleteReservationAsync(Guid reservationId);
    }
}
