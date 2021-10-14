using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace HotelResFE.ViewModels
{
    public class HotelDetailsViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private IHotelsService _hotelsService;
        private IReservationsService _reserverationsService;
        private Hotel _selectedHotel;
        private Reservation _reservation;
        private Room _room;
        private ObservableCollection<Reservation> _reserverations;

        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _selectedStartDate;
        private DateTime _selectedEndDate;
        private ObservableCollection<DateTime> _datesToBeDisabled;
        private ObservableCollection<DateTime> _datesToBeEnabled;

        private bool _includeTransport;
        private bool _includePool;
        private bool _includeBreakfast;
        private bool _includeAll;

        private int _selectedSize;
        

        public Hotel SelectedHotel
        {
            get { return _selectedHotel; }
            set { SetProperty(ref _selectedHotel, value); }
        }
        public Room Room
        {
            get { return _room; }
            set { SetProperty(ref _room, value); }
        }
        public ObservableCollection<Reservation> Reservations
        {
            get { return _reserverations; }
            set { SetProperty(ref _reserverations, value); }
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
        public DateTime SelectedStartDate
        {
            get { return _selectedStartDate; }
            set 
            { 
                SetProperty(ref _selectedStartDate, value); 
                RaisePropertyChanged(nameof(DatesToBeEnabled)); //Går att generalisera mer.
                RaisePropertyChanged(nameof(ArrivalIsSet));
                RaisePropertyChanged(nameof(SelectedEndDate));
            }
        }
        public DateTime SelectedEndDate
        {
            get { return DatesToBeEnabled.Contains(_selectedEndDate) ? _selectedEndDate : _selectedStartDate.AddDays(7); }
            set { SetProperty(ref _selectedEndDate, DatesToBeEnabled.Contains(value) ? value : _selectedStartDate.AddDays(7)); }
        }
        public ObservableCollection<DateTime> DatesToBeDisabled
        {
            get { return _datesToBeDisabled; }
            set { SetProperty(ref _datesToBeDisabled, value);}
        }
        public ObservableCollection<DateTime> DatesToBeEnabled
        {
            get
            {
                DateTime d1 = SelectedStartDate.Date.AddDays(7);
                DateTime d2 = SelectedStartDate.Date.AddDays(14);
                ObservableCollection<DateTime> dtbe = new ObservableCollection<DateTime>();
                if (DatesToBeDisabled.Any(d => d.Date <= d1 && d.Date >= SelectedStartDate.Date)) //Om detta är sant innebär det att alla rum är upptagna någon av dagarna i bokningsperioden...
                    return dtbe;
                
                dtbe.Add(d1);

                if (DatesToBeDisabled.Any(d => d.Date <= d2 && d.Date >= d1))
                    return dtbe;

                dtbe.Add(d2);

                return dtbe;
            }
            set { SetProperty(ref _datesToBeEnabled, value); }
        }
        public bool ArrivalIsSet
        {
            get { return SelectedStartDate >= StartDate; }
            
        }
        
        public bool IncludeTransport
        {
            get { return _includeTransport; }
            set { SetProperty(ref _includeTransport, value); RaisePropertyChanged(nameof(TotalAmount)); }
        }
        public bool IncludePool
        {
            get { return _includePool; }
            set { SetProperty(ref _includePool, value); RaisePropertyChanged(nameof(TotalAmount)); }
        }
        public bool IncludeBreakFast
        {
            get { return _includeBreakfast; }
            set { SetProperty(ref _includeBreakfast, value); RaisePropertyChanged(nameof(TotalAmount)); }
        }
        public bool IncludeAll
        {
            get { return _includeAll; }
            set { SetProperty(ref _includeAll, value); RaisePropertyChanged(nameof(TotalAmount)); }
        }


        

        public int SelectedSize
        {
            get { return _selectedSize; }
            set
            {
                SetProperty(ref _selectedSize, value);
                RaisePropertyChanged("ValueAs0");
                RaisePropertyChanged("ValueAs1");
                RaisePropertyChanged("ValueAs2");
                RaisePropertyChanged(nameof(TotalAmount));
                LoadUnAvailableDates();
            }
        }
        public bool ValueAs0
        {
            get { return SelectedSize.Equals(0); }
            set { SelectedSize = 0; }
        }
        public bool ValueAs1
        {
            get { return SelectedSize.Equals(1); }
            set { SelectedSize = 1; }
        }
        public bool ValueAs2
        {
            get { return SelectedSize.Equals(2); }
            set { SelectedSize = 2; }
        }

        public DelegateCommand<Reservation> MakeReservationCommand { get; private set; }
        public int TotalAmount 
        { 
            get { return CalculateTotalAmount(); } 
            
        }

        public HotelDetailsViewModel(IEventAggregator aggregator, IHotelsService hotelsService, IReservationsService reservationsService)
        {
            _eventAggregator = aggregator;
            _hotelsService = hotelsService;
            _reserverationsService = reservationsService;

            _selectedHotel = new();
            _reservation = new();
            _room = new();

            _startDate = DateTime.Today.Date;
            _endDate = DateTime.Today.Date.AddDays(366);
            
            _datesToBeDisabled = new();
            _selectedSize = 1;

            _eventAggregator.GetEvent<SelectedHotelEvent>().Subscribe(LoadHotel);
            _eventAggregator.GetEvent<ReservationMadeEvent>().Subscribe(LoadReservations);

            MakeReservationCommand = new DelegateCommand<Reservation>(MakeReservation);

        }

        private void MakeReservation(Reservation res)
        {
            res = new();
            res.IncludeAll = IncludeAll;
            res.IncludeBreakfast = IncludeBreakFast;
            res.IncludePool = IncludePool;
            res.IncludeTransport = IncludeTransport;
            res.StartDate = SelectedStartDate;
            res.EndDate = SelectedEndDate;
            res.RoomId = FindAvailableRoom();
            res.TotalAmount = CalculateTotalAmount();
            _reserverationsService.PostReservationAsync(res);

            _eventAggregator.GetEvent<ReservationMadeEvent>().Publish();
        }
        private Guid FindAvailableRoom()
        {
            Room room = new Room();
            List<Room> rooms = SelectedHotel.Rooms.Where(r => r.Size == SelectedSize).ToList();
            List<Reservation> ress = Reservations.Where(res => res.StartDate >= SelectedStartDate && res.StartDate <= SelectedEndDate || res.EndDate <= SelectedEndDate && res.EndDate >= SelectedStartDate).ToList();
            room = rooms.First(r => ress.FirstOrDefault(res => res.RoomId == r.RoomId) == null);
            
            return room.RoomId;
        }
        
        private int CalculateTotalAmount()
        {
            int totam = 0;
            if (SelectedSize == 0)
                totam += SelectedHotel.PriceSingleRoom;
            else if (SelectedSize == 1)
                totam += SelectedHotel.PriceDoubleRoom;
            else if (SelectedSize == 2)
                totam += SelectedHotel.PriceSuite;

            if (IncludeAll)
                totam += SelectedHotel.PriceAllInclusive;
            if (IncludeBreakFast)
                totam += SelectedHotel.PriceBreakfast;
            if (IncludeTransport)
                totam += SelectedHotel.PriceTransport;
            if (IncludePool)
                totam += SelectedHotel.PricePool;
            return totam;
        }

        private async void LoadReservations()
        {
            if (SelectedHotel == null)
                return;

            var reservations = await _reserverationsService.GetReservationsByHotelIdAsync(SelectedHotel.HotelId);
            if (reservations == null)
                return;

            Reservations = new();
            foreach (Reservation r in reservations)
            {
                Reservations.Add(r);
            }
            LoadUnAvailableDates();
        }

        private void LoadUnAvailableDates()
        {
            if (SelectedHotel == null || Reservations == null)
                return;

            _datesToBeDisabled.Clear();
            
            List<Reservation> reservationsBySize = Reservations.Where(r => r.Room.Size == SelectedSize).ToList();
            List<Room> roomsBySize = SelectedHotel.Rooms.Where(r => ((int)r.Size) == SelectedSize).ToList();
            DatesToBeDisabled = new();
            for (DateTime d = StartDate; d <= EndDate; d = d.AddDays(1))
            {

                DatesToBeDisabled.Add(d);
                foreach (Room room in roomsBySize)
                {
                    if (reservationsBySize.Where(res => d >= res.StartDate && d <= res.EndDate).FirstOrDefault(res => res.RoomId == room.RoomId) == null)
                        DatesToBeDisabled.Remove(d);
                    
                }
                    
            }
            
            RaisePropertyChanged(nameof(DatesToBeDisabled)); //Så att view fattar att den ska kolla listan efter att den skrivits om.

        }

        private void LoadHotel(Hotel hotel)
        {
            SelectedHotel = hotel;
            LoadReservations();
        }
    }

    //Denna placeras i denna fil av "säkerhetsskäl"
    public class DateTimesToBoolConverter : IMultiValueConverter
    {
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            (values == null || values.Length < 2) ? true :
                (values[0] as IEnumerable<DateTime>)?.Contains((DateTime)values[1]) ?? true;



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();


    }

}
