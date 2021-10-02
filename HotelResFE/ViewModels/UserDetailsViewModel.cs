using HotelResFE.Models;
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
        private IEventAggregator _aggregator;
        
        public User User
        { 
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public UserDetailsViewModel(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            _user = new();

        }
    }
}
