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
        private string _adress;
        private string _phoneNumber;
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
        public string Adress
        { 
            get { return _adress; }
            set { SetProperty(ref _adress, value); }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }
        public string Password
        {
            
            set { SetProperty(ref _password, SecurityService.Garble(value)); }
        }
        public string PassControl
        {
            
            set { SetProperty(ref _passControl, SecurityService.Garble(value)); }
        }

        public DelegateCommand<User> CreateUserCommand { get; private set; }

        public RegisterViewModel(IEventAggregator eventAggregator, IUserService userService)
        {
            _eventAggregator = eventAggregator;
            _userService = userService;
            CreateUserCommand = new DelegateCommand<User>(CreateUser, CanCreateUser);
        }
        private bool CanCreateUser(User user)
        {
            bool canDo = true;
            if (_password != _passControl)
                canDo = false;

            return canDo;
        }
        private async void CreateUser(User user)
        {
            user = new();
            user.FirstName = _firstName;
            user.LastName = _lastName;
            user.Email = _email;
            user.Adress = _adress;
            user.PhoneNr = _phoneNumber;
            user.Password = _password;

            HttpStatusCode r = await _userService.RegisterNewUserAsync(user);

            if(r == HttpStatusCode.OK)
            {
                _eventAggregator.GetEvent<RegisteredEvent>().Publish();
                _eventAggregator.GetEvent<LoggedInEvent>().Publish();
            }
        }

    }
}
