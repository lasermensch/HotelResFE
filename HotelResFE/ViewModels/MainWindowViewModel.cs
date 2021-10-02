using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace HotelResFE.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title = "Titel!";
        private bool _isLoggedIn;
        private IEventAggregator _aggregator;
        private IRegionManager _manager;

        public Visibility ToggleUserpageBtnVisibility
        {
            get { return _isLoggedIn ? Visibility.Visible : Visibility.Hidden; }
        }
        public Visibility ToggleLoginBtnVisibility
        {
            get { return _isLoggedIn ? Visibility.Hidden : Visibility.Visible; }
        }

        public MainWindowViewModel(IEventAggregator aggregator, IRegionManager manager)
        {
            _isLoggedIn = false;
            _aggregator = aggregator;
            _manager = manager;
        }
    }
}
