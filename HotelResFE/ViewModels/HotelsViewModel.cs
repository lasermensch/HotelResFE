using HotelResFE.DataServices;
using HotelResFE.Models;
using HotelResFE.Views;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.ViewModels
{
    public class HotelsViewModel : BindableBase
    {
        private ObservableCollection<Hotel> _hotels;
        private Hotel _selectedHotel;
        private HotelImage _selectedImage;

        private ObservableCollection<HotelImage> _images;
        private IEventAggregator _aggregator;
        private IHotelsService _service;



        public ObservableCollection<Hotel> Hotels
        {
            get { return _hotels; }
            set { SetProperty(ref _hotels, value); }
        }
        public Hotel SelectedHotel
        {
            get { return _selectedHotel; }
            set { SetProperty(ref _selectedHotel, value); }
        }
        public HotelImage SelecedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }
        public ObservableCollection<HotelImage> Images
        {
            get
            {
                if (_selectedHotel.Images != null)
                    return new ObservableCollection<HotelImage>(_selectedHotel.Images);
                else
                    return new ObservableCollection<HotelImage>();
            } 
            set { SetProperty(ref _images, value); }
        }


        public HotelsViewModel(IEventAggregator aggregator, IHotelsService service)
        {
            _aggregator = aggregator;
            _service = service;
            _hotels = new();
            _images = new();
            _selectedHotel = new();
            
            LoadHotels();
        }

        private async void LoadHotels()
        {
            IEnumerable<Hotel> data = await _service.GetHotelsAsync();
            if (data == null)
                return;
            foreach (Hotel h in data)
            {
                _hotels.Add(h);
            }
        }
    }
}
