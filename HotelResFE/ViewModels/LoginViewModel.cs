using HotelResFE.DataServices;
using HotelResFE.Events;
using HotelResFE.Models;
using HotelResFE.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelResFE.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly ILoginService _service;
        private readonly IEventAggregator _aggregator;
        
        private string _username;
        private string _password;
        
        public DelegateCommand<LoginCreds> PostLoginCommand { get; private set; }

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); PostLoginCommand.RaiseCanExecuteChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value: Login.Pwd); PostLoginCommand.RaiseCanExecuteChanged(); }
        }

        public LoginViewModel(ILoginService service, IEventAggregator aggregator)
        {
            _service = service;
            _aggregator = aggregator;
            _username = "";
            _password = "";

            PostLoginCommand = new DelegateCommand<LoginCreds>(PostLogin, CanPostLogin);
        }

        private async void PostLogin(LoginCreds obj)
        {
            LoginCreds creds = new LoginCreds();
            creds.Username = _username;
            creds.Password = _password;

            string resp = await _service.LoginAsync(creds);

            if(resp == null)
            {
                MessageBox.Show("Failed Login Attempt!");
            }
            else
            {
                _aggregator.GetEvent<LoggedInEvent>().Publish();
                MessageBox.Show("Login Attempt successful!");
            }
        }

        private bool CanPostLogin(LoginCreds arg)
        {
            bool canDo = true;
            if (String.IsNullOrWhiteSpace(_username) || String.IsNullOrWhiteSpace(_password))
                canDo = false;

            return canDo;
        }
    }

    
}
