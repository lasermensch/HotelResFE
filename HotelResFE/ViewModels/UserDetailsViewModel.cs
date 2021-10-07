using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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

        public UserDetailsViewModel(IEventAggregator eventAggregator, IUserService userService)
        {
            _eventAggregator = eventAggregator;
            _userService = userService;
            _user = LoadUser().Result;
            UpdateUserCommand = new DelegateCommand<User>(UpdateUser);
            DeleteUserCommand = new DelegateCommand<User>(DeleteUser);
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
            ////_userService;
        }

        private async Task<User> LoadUser()
        {
            User u = await _userService.GetUserAsync();
            return u;
            
        }
    }
}
