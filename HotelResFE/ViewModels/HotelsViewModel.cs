using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using HotelResFE.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private IRegionManager _regionManager;

        public ObservableCollection<Hotel> Hotels
        {
            get { return _hotels; }
            set { SetProperty(ref _hotels, value); }
        }
        public Hotel SelectedHotel
        {
            get { return _selectedHotel; }
            set { SetProperty(ref _selectedHotel, value); _aggregator.GetEvent<SelectedHotelEvent>().Publish(value); }
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

        public DelegateCommand<string> SortCommand { get; private set; }
        public DelegateCommand NavigateToHotelDetailsCommand { get; private set; }

        public HotelsViewModel(IEventAggregator aggregator, IHotelsService service, IRegionManager regionManager)
        {
            _aggregator = aggregator;
            _service = service;
            _regionManager = regionManager;
            _hotels = new();
            _images = new();
            _selectedHotel = new();

            SortCommand = new DelegateCommand<string>(SortHotels);
            NavigateToHotelDetailsCommand = new DelegateCommand(NavigateToHotelDetails);
            LoadHotels();
        }

        private void NavigateToHotelDetails()
        {
            _regionManager.RequestNavigate("ContentRegion", "HotelDetails");
        }

        private void SortHotels(string sortingparam)
        {
            List<Hotel> slist = new() ;
            switch(sortingparam)
            {
                case "SortByPrice":
                    slist = Hotels.OrderBy(hotel => hotel.Rooms.Min(room => room.Price)).ToList();
                    break;
                case "SortByRating":
                    slist = Hotels.OrderBy(hotel => hotel.Rating).ToList();
                    break;
                case "SortBySize":
                    slist = Hotels.OrderBy(hotel => hotel.Rooms.Count).ToList();
                    break;
                default:
                    break;

                    
            }
           if (slist.Count != 0)
           {
                Hotels.Clear();
                foreach (Hotel h in slist)
                {
                    Hotels.Add(h);
                }
            }
            
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
