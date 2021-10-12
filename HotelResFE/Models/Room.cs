using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace HotelResFE.Models
{
    public class Room
    {   
        public Guid RoomId { get; set; }
        public Guid HotelId { get; set; }
        
        public int Size { get; set; }

        //Navprops
        public Hotel Hotel { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }

    
}
