using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelResFE.Models
{
    public class RoomImage
    {
        
        public Guid ImageId { get; set; }
        
        public Guid RoomId { get; set; }
        public string Uri { get; set; }

        //Navprop
        public Room Room { get; set; }
    }
}
