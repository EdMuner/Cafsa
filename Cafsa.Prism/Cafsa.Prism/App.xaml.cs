using Prism;
using Prism.Ioc;
using Cafsa.Prism.ViewModels;
using Cafsa.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cafsa.Common.Services;
using Newtonsoft.Json;
using Cafsa.Common.Models;
using Cafsa.Common.Helpers;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Cafsa.Prism
{
    public partial class App
    {      
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTYzMDM0QDMxMzcyZTMzMmUzME5QSmJhTjkyM2krVXRKT0lGYVdmdC9RbGhJMEhRcXM1ZUlMTGwyaWV1RGc9");
            InitializeComponent();

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            if (Settings.IsRemembered && token?.Expiration > DateTime.Now)
            {
                await NavigationService.NavigateAsync("/CafsaMasterDetailPage/NavigationPage/ActivitiesPage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //injecta navegacion de servicio
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ActivitiesPage, ActivitiesPageViewModel>();
            containerRegistry.RegisterForNavigation<ActivityPage, ActivityPageViewModel>();
            containerRegistry.RegisterForNavigation<ServicesPage, ServicesPageViewModel>();
            containerRegistry.RegisterForNavigation<ServicePage, ServicePageViewModel>();
            containerRegistry.RegisterForNavigation<ActivityTabbedPage, ActivityTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<CafsaMasterDetailPage, CafsaMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
        }
    }
}
