using System;
using Cafsa.Common.Models;
using Prism.Commands;
using Prism.Navigation;

namespace Cafsa.Prism.ViewModels
{
    public class ServiceItemViewmodel : ServiceResponse
    {
        private readonly INavigationService _navigationService;
        public DelegateCommand _selectServiceCommand;

        public ServiceItemViewmodel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectServiceCommand => _selectServiceCommand ?? (_selectServiceCommand = new DelegateCommand(SelectService));

        private async void SelectService()
        {
            var parameters = new NavigationParameters
            {
                { "service", this }
            };

            await _navigationService.NavigateAsync("ServicePage", parameters);
        }
    }
}

