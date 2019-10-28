using Cafsa.Common.Models;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class ServicesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ActivityResponse _activity;
        private ObservableCollection<ServiceItemViewmodel> _services;

        public ServicesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Services";
        }

        public ObservableCollection<ServiceItemViewmodel> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }
        public ActivityResponse Activity
        {
            get => _activity;
            set => SetProperty(ref _activity, value);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("activity"))
            {
                _activity = parameters.GetValue<ActivityResponse>("activity");
                LoadContracts();

            }
        }

        private void LoadContracts()
        {
            Services = new ObservableCollection<ServiceItemViewmodel>(Activity.Services.Select(s => new ServiceItemViewmodel(NavigationService)
            {
                StartDate = s.StartDateLocal,
                Client = s.Client,
                Price = s.Price,
                Remarks = s.Remarks,
                IsActive = s.IsActive,

            }).ToList());
        }
    }
}
