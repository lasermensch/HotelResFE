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
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _selectedDate;
        private bool _includeTransport;
        private bool _includePool;
        private bool _includeBreakfast;
        private bool _includeAll;

        public bool IncludeTransport
        {
            get { return _includeTransport; }
            set { SetProperty(ref _includeTransport, value); }
        }
        public bool IncludePool
        {
            get { return _includePool; }
            set { SetProperty(ref _includePool, value); }
        }
        public bool IncludeBreakFast
        {
            get { return _includeBreakfast; }
            set { SetProperty(ref _includeBreakfast, value); }
        }
        public bool IncludeAll
        {
            get { return _includeAll; }
            set { SetProperty(ref _includeAll, value); }
        }

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

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }

        public HotelDetailsViewModel(IEventAggregator aggregator, IHotelsService service)
        {
            _aggregator = aggregator;
            _service = service;
            _hotel = new();
            _reservation = new();
            _room = new();
            _startDate = DateTime.Today;
            _endDate = DateTime.Today.AddDays(366);

        }
    }
}
