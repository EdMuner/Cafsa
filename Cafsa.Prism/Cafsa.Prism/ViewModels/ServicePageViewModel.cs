using Cafsa.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class ServicePageViewModel : ViewModelBase
    {
        private ServiceResponse _service;
        public ServicePageViewModel(
              INavigationService navigationService) : base(navigationService)
        {
            Title = "Service";
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
                Title = $"Service to: {Service.Client.FullName}";
            }
        }
    }
}
