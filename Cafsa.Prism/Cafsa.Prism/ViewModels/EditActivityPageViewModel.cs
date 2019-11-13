using Cafsa.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class EditActivityPageViewModel : ViewModelBase
    {
        public EditActivityPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.AddActivity;
        }
    }
}
