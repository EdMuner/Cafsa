using Prism;
using Prism.Ioc;
using Cafsa.Prism.ViewModels;
using Cafsa.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cafsa.Common.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Cafsa.Prism
{
    public partial class App
    {      
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //injecta navegacion de servicio
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ActivitiesPage, ActivitiesPageViewModel>();
            containerRegistry.RegisterForNavigation<ActivityPage, ActivityPageViewModel>();
        }
    }
}
