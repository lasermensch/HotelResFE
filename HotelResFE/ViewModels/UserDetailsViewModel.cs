﻿using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.ViewModels
{
    public class UserDetailsViewModel: BindableBase 
    {
        private User _user;
        private IEventAggregator _eventAggregator;
        private IUserService _userService;
        private IReservationsService _reservationsService;
        private string _newPassword;
        private string _passControl;

        public User User
        { 
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
        public string NewPassword
        {
            
            set { SetProperty(ref _newPassword, value); }
        }
        public string PassControl
        {
            set { SetProperty(ref _passControl, value); }
        }
        

        public DelegateCommand<User> DeleteUserCommand { get; private set; }
        public DelegateCommand<User> UpdateUserCommand { get; private set; }
        public DelegateCommand<Guid> DeleteReservationCommand { get; private set; }
        

        public UserDetailsViewModel(IEventAggregator eventAggregator, IUserService userService, IReservationsService reservationsService)
        {
            _eventAggregator = eventAggregator;
            _userService = userService;
            _reservationsService = reservationsService;
            _user = new();
            UpdateUserCommand = new DelegateCommand<User>(UpdateUser);
            DeleteUserCommand = new DelegateCommand<User>(DeleteUser);
            DeleteReservationCommand = new DelegateCommand<Guid>(DeleteReservation);
            _eventAggregator.GetEvent<LoggedInEvent>().Subscribe(LoadUser);
        }

        private async void DeleteReservation(Guid reservationId)
        {
            await _reservationsService.DeleteReservationAsync(reservationId);
        }

        private async void DeleteUser(User user)
        {
            var response = await _userService.DeleteUserAsync();
            if (response == System.Net.HttpStatusCode.NoContent)
                _eventAggregator.GetEvent<LoggedOutEvent>().Publish();
        }

        private void UpdateUser(User user)
        {
            user = _user;
            user.Password = _passControl;
        }
        private async void LoadUser()
        {
            User = await _userService.GetUserAsync();
        }
       
    }
}
