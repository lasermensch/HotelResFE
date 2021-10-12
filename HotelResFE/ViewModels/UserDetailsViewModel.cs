using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelResFE.ViewModels
{
    public class UserDetailsViewModel: BindableBase 
    {
        private User _user;
        private Reservation _selectedReservation;
        private IEventAggregator _eventAggregator;
        private IUserService _userService;
        private IReservationsService _reservationsService;
        private string _newPassword;
        private string _passControl;
        private ObservableCollection<Reservation> _reservations;

        public User User
        { 
            get { return _user; }
            set { SetProperty(ref _user, value); 
                RaisePropertyChanged(nameof(FirstName));  //Fult, men fortfarande mindre kod än att göra tre miljarder variabler.
                RaisePropertyChanged(nameof(LastName));
                RaisePropertyChanged(nameof(Email));
                RaisePropertyChanged(nameof(Address));
                RaisePropertyChanged(nameof(PhoneNr));
            }
        }
        public string FirstName
        {
            get { return _user.FirstName; }
            set { _user.FirstName = value; RaisePropertyChanged(nameof(FirstName)); } //SetProperty() Fungerar inte här, eftersom man inte kan sätta ref framför en referens till ett annat objekts proppar
        }
        public string LastName
        {
            get { return _user.LastName; }
            set { _user.LastName = value; RaisePropertyChanged(nameof(LastName)); }
        }
        public string Email
        {
            get { return _user.Email; }
            set { _user.Email = value; RaisePropertyChanged(nameof(Email)); }
        }
        public string Address
        {
            get { return _user.Adress; }
            set { _user.Adress = value; RaisePropertyChanged(nameof(Address)); }
        }
        public string PhoneNr
        {
            get { return _user.PhoneNr; }
            set { _user.PhoneNr = value; RaisePropertyChanged(nameof(PhoneNr)); }
        }

        public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set { SetProperty(ref _selectedReservation, value); }
        }
        public ObservableCollection<Reservation> Reservations
        { 
            get { return _reservations; }
            set { SetProperty(ref _reservations, value); }
        }
        public string NewPassword
        {
            
            set { SetProperty(ref _newPassword, SecurityService.Garble(value)); }
        }
        public string PassControl
        {
            set { SetProperty(ref _passControl, SecurityService.Garble(value)); }
        }
        

        public DelegateCommand<User> DeleteUserCommand { get; private set; }
        public DelegateCommand<User> EditUserCommand { get; private set; }
        public DelegateCommand<Guid?> DeleteReservationCommand { get; private set; }
        

        public UserDetailsViewModel(IEventAggregator eventAggregator, IUserService userService, IReservationsService reservationsService)
        {
            _eventAggregator = eventAggregator;
            _userService = userService;
            _reservationsService = reservationsService;
            _user = new();
            
            EditUserCommand = new DelegateCommand<User>(EditUser);
            DeleteUserCommand = new DelegateCommand<User>(DeleteUser);
            DeleteReservationCommand = new DelegateCommand<Guid?>(DeleteReservation);
            _eventAggregator.GetEvent<LoggedInEvent>().Subscribe(LoadUser);
            _eventAggregator.GetEvent<LoggedOutEvent>().Subscribe(() => _user = new());
            _eventAggregator.GetEvent<ReservationMadeEvent>().Subscribe(LoadUser); //Fulkoppling.
        }

        private async void DeleteReservation(Guid? reservationId)
        {
            if (reservationId == null)
                return;

            await _reservationsService.DeleteReservationAsync(reservationId.Value);
            Reservations.Remove(Reservations.FirstOrDefault(r => r.ReservationId == reservationId));
            RaisePropertyChanged(nameof(Reservations));
        }

        private async void DeleteUser(User user)
        {
            var response = await _userService.DeleteUserAsync();
            if (response == System.Net.HttpStatusCode.NoContent)
                _eventAggregator.GetEvent<DeletedUserEvent>().Publish();
        }

        private async void EditUser(User user)
        {
            user = _user;
            if(_newPassword != _passControl)
                { MessageBox.Show("New Password not confirmed correctly!"); return; }
            if (_newPassword != null)
                user.Password = _newPassword;

            await _userService.EditUserAsync(user);
        }
        private async void LoadUser()
        {
            User = await _userService.GetUserAsync();
            Reservations = new();
            foreach (Reservation r in User.Reservations)
            {
                Reservations.Add(r);
            }
        }
       
    }
}
