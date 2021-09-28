using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.Models
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        
        public Guid RoomId { get; set; }
        
        public Guid UserId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public byte Addons { get; set; }

        //Navprops
        public User User { get; set; }
        public Room Room { get; set; }
    }
}
