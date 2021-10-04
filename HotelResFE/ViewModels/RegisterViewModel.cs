using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelResFE.ViewModels
{
    public class RegisterViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private IUserService _userService;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _userName;
        private string _password;
        private string _passControl;

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public string PassControl
        {
            get { return _passControl; }
            set { SetProperty(ref _passControl, value); }
        }

        public DelegateCommand<User> CreateUserCommand { get; private set; }

        public RegisterViewModel(IEventAggregator eventAggregator, IUserService userService)
        {
            _eventAggregator = eventAggregator;
            _userService = userService;
            
        }

        private async void CreateUser(User user)
        {
            user = new();
            user.FirstName = _firstName;
            user.LastName = _lastName;
            user.Email = _email;
            user.PhoneNr = _phoneNumber;
            user.UserName = _userName;
            user.Password = _password;

            HttpStatusCode r = await _userService.RegisterNewUserAsync(user);

            if(r == HttpStatusCode.OK)
            {
                _eventAggregator.GetEvent<RegisteredEvent>().Publish();
            }
        }

    }
}
