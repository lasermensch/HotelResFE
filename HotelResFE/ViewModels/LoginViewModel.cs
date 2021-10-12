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
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace HotelResFE.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IUserService _userService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        
        private string _email;
        private string _password;
        
        public DelegateCommand<LoginCreds> PostLoginCommand { get; private set; }
        
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); PostLoginCommand.RaiseCanExecuteChanged(); }
        }
       
        public string Password
        {
            
            set { SetProperty(ref _password, SecurityService.Garble(value)); PostLoginCommand.RaiseCanExecuteChanged(); }
        }

        

        public LoginViewModel(IUserService userService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _userService = userService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _email = "";
            _password = "";

            PostLoginCommand = new DelegateCommand<LoginCreds>(PostLogin, CanPostLogin);
            _eventAggregator.GetEvent<RegisteredEvent>().Subscribe(PostLogin);
            //_eventAggregator.GetEvent<LoggedOutEvent>().Subscribe();
        }

       

        private async void PostLogin(LoginCreds creds)
        {
            if (creds == null)
                creds = new();
            if (String.IsNullOrWhiteSpace(creds.Email) && String.IsNullOrWhiteSpace(creds.Password))
            {
                creds.Email = _email;
                creds.Password = _password;
            }

            string resp = await _userService.LoginAsync(creds);

            if(resp == null)
            {
                MessageBox.Show("Email or Password is incorrect!");
            }
            else
            {
                _eventAggregator.GetEvent<LoggedInEvent>().Publish();
                _regionManager.RequestNavigate("ContentRegion", "Hotels");
            }
        }

        private bool CanPostLogin(LoginCreds arg)
        {
            bool canDo = true;
            if (String.IsNullOrWhiteSpace(_email) || String.IsNullOrWhiteSpace(_password))
                canDo = false;

            return canDo;
        }

        
    }

    
}
