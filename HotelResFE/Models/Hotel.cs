using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.Models
{
    public class Hotel
    {   public Guid HotelId { get; set; }
     
        public string HotelName { get; set; }
     
        public string Adress { get; set; }
     
        public string Email { get; set; }
        public double Rating { get; set; }
     
        public string PhoneNr { get; set; }
        public string WebPage { get; set; }

        //Navprops
        public ICollection<Room> Rooms { get; set; }
        public ICollection<HotelImage> Images { get; set; }
    }
}
