using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public DelegateCommand<string> NavigateToHotelsView { get; private set; }
        public DelegateCommand<string> NavigateToLoginView { get; private set; }
        public DelegateCommand<string> NavigateToRegisterView { get; private set; }
        public DelegateCommand<string> NavigateToUserDetailsView { get; private set; }

        public Visibility ToggleUserpageBtnVisibility
        {
            get { return _isLoggedIn ? Visibility.Visible : Visibility.Hidden; }
        }
        public Visibility ToggleLoginBtnVisibility
        {
            get { return _isLoggedIn ? Visibility.Hidden : Visibility.Visible; }
        }

        public MainWindowViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _isLoggedIn = false;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            NavigateToHotelsView = new DelegateCommand<string>(Navigate);
            NavigateToLoginView = new DelegateCommand<string>(Navigate);
            NavigateToRegisterView = new DelegateCommand<string>(Navigate);
            NavigateToUserDetailsView = new DelegateCommand<string>(Navigate);

        }

        private void Navigate(string path)
        {
            _regionManager.RequestNavigate("ContentRegion", path);
        }
    }
}
