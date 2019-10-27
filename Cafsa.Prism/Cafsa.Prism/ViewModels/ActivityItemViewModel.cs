using Cafsa.Common.Models;
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
            //Se crea un objeto de navegación y se lenvia las actividades del referee especifico         
            var parameters = new NavigationParameters
            {
                {"activity", this }
            };

            await _navigationService.NavigateAsync("ActivityPage", parameters);

        }
    }
}
