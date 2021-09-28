using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelResFE.Models
{
    public class HotelImage
    {
        
        public Guid ImageId { get; set; }
        
        public Guid HotelId { get; set; }
        public string Uri { get; set; }

        //Navprop
        public Hotel Hotel { get; set; }
    }
}
