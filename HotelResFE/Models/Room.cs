using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.Models
{
    public class Room
    {
        
        public Guid RoomId { get; set; }
        
        public Guid HotelId { get; set; }
        
        public int Size { get; set; }
        
        public int Price { get; set; }
        public int Rating { get; set; }
        
        public int NrOfVotes { get; set; } = 0;

        //Navprops
        public Hotel Hotel { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<RoomImage> Images { get; set; }
    }
}
