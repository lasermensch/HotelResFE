using HotelResFE.DataServices;
using HotelResFE.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.ViewModels
{
    public class HotelDetailsViewModel : BindableBase
    {
        private IEventAggregator _aggregator;
        private IHotelsService _service;
        private Hotel _hotel;
        private Reservation _reservation;
        private Room _room;
        
        public Hotel Hotel
        {
            get { return _hotel; }
            set { SetProperty(ref _hotel, value); }
        }
        public Room Room
        {
            get { return _room; }
            set { SetProperty(ref _room, value); }
        }
        

        public HotelDetailsViewModel(IEventAggregator aggregator, IHotelsService service)
        {
            _aggregator = aggregator;
            _service = service;
            _hotel = new();
            _reservation = new();
            _room = new();

        }
    }
}
