using Cafsa.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class ActivityPageViewModel : ViewModelBase
    {

        
        private ActivityResponse _activity;
        public ActivityPageViewModel(
            INavigationService navigationService ) : base(navigationService)
        {
            Title = "Activity";
        }
        //este metodo sirve para que cuando yo navegue capturar la actividad y la bindamos para traer todos los campos referentes
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
                Activity = parameters.GetValue<ActivityResponse>("activity");
                Title = $"Activity: { Activity.Neighborhood}";
            }
        }


    }
}
