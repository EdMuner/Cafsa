using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Cafsa.Prism.ViewModels
{
    public class ActivityItemViewModel : ActivityResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectActivityCommand;

        public ActivityItemViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectActivityCommand => _selectActivityCommand ?? (_selectActivityCommand = new DelegateCommand(SelectActivity));

        private async void SelectActivity()
        {
            Settings.Activity =JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("ActivityTabbedPage");

        }
    }
}
