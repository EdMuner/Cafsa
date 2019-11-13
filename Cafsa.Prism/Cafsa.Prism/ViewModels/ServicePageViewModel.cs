using Cafsa.Common.Models;
using Cafsa.Prism.Helpers;
using Prism.Navigation;

namespace Cafsa.Prism.ViewModels
{
    public class ServicePageViewModel : ViewModelBase
    {
        private ServiceResponse _service;
        public ServicePageViewModel(
              INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Service;
        }
        public ServiceResponse Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("service"))
            {
                Service = parameters.GetValue<ServiceResponse>("service");
                Title = $"Servicio a: {Service.Client.FullName}";
            }
        }
    }
}
