using HotelResFE.DataServices;
using HotelResFE.Views;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HotelResFE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IUserService, UserService>();
            containerRegistry.RegisterScoped<IHotelsService, HotelsService>();

            containerRegistry.RegisterForNavigation<Login>();
            containerRegistry.RegisterForNavigation<Hotels>();
            containerRegistry.RegisterForNavigation<UserDetails>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var regionManager = Container.Resolve<IRegionManager>();
            var contentRegion = regionManager.Regions["ContentRegion"];
            var homeView = Container.Resolve<Home>();
            var hotelsView = Container.Resolve<Hotels>();
            var loginView = Container.Resolve<Login>();
            var userDetailsView = Container.Resolve<UserDetails>();
            var hotelDetailsView = Container.Resolve<HotelDetails>();
            var registerView = Container.Resolve<Register>();

            contentRegion.Add(homeView);
            contentRegion.Add(loginView);
            contentRegion.Add(hotelsView);
            contentRegion.Add(userDetailsView);
            contentRegion.Add(hotelDetailsView);
            contentRegion.Add(registerView);
            
        }
    }
}
