using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HotelResFE.DataServices;
using HotelResFE.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace HotelResFE.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        
        private bool _isLoggedIn;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private IUserService _userService;

        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand<string> NavigateToHotelsViewCommand { get; private set; }
        public DelegateCommand<string> NavigateToLoginViewCommand { get; private set; }
        public DelegateCommand<string> NavigateToRegisterViewCommand { get; private set; }
        public DelegateCommand<string> NavigateToUserDetailsViewCommand { get; private set; }

        public Visibility ToggleUserpageBtnVisibility
        {
            get { return _isLoggedIn ? Visibility.Visible : Visibility.Hidden; }
        }
        public Visibility ToggleLoginBtnVisibility
        {
            get { return _isLoggedIn ? Visibility.Hidden : Visibility.Visible; }
        }

        public MainWindowViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IUserService userService)
        {
            _isLoggedIn = false;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _userService = userService;

            LogoutCommand = new DelegateCommand(LogOut);

            NavigateToHotelsViewCommand = new DelegateCommand<string>(Navigate);
            NavigateToLoginViewCommand = new DelegateCommand<string>(Navigate);
            NavigateToRegisterViewCommand = new DelegateCommand<string>(Navigate);
            NavigateToUserDetailsViewCommand = new DelegateCommand<string>(Navigate);

            _eventAggregator.GetEvent<LoggedInEvent>().Subscribe(IsLoggedIn);
            _eventAggregator.GetEvent<DeletedUserEvent>().Subscribe(LogOut);
            
        }

        private void LogOut()
        {
            _userService.LogOut();
            _isLoggedIn = false;
            RaisePropertyChanged(nameof(ToggleLoginBtnVisibility));
            RaisePropertyChanged(nameof(ToggleUserpageBtnVisibility));
            Navigate("Home");
        }

        private void IsLoggedIn()
        {
            _isLoggedIn = true;
            RaisePropertyChanged(nameof(ToggleLoginBtnVisibility));
            RaisePropertyChanged(nameof(ToggleUserpageBtnVisibility));
        }

        

        private void Navigate(string path)
        {
            _regionManager.RequestNavigate("ContentRegion", path);
        }
    }
}
