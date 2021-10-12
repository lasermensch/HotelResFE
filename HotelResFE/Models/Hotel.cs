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
        public string PhoneNr { get; set; }
        public string WebPage { get; set; }

        public double Rating { get; set; }
        public int NrOfVotes { get; set; }

        public int PriceSingleRoom { get; set; }
        public int PriceDoubleRoom { get; set; }
        public int PriceSuite { get; set; }
        public int PriceTransport { get; set; }
        public int PriceBreakfast { get; set; }
        public int PricePool { get; set; }
        public int PriceAllInclusive { get; set; }

        //Navprops
        public ICollection<Room> Rooms { get; set; }
        public ICollection<HotelImage> Images { get; set; }
    }
}
