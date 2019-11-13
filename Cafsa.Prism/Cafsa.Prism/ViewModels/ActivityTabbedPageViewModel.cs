using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Newtonsoft.Json;
using Prism.Navigation;

namespace Cafsa.Prism.ViewModels
{
    public class ActivityTabbedPageViewModel : ViewModelBase
    {
        public ActivityTabbedPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            var activity = JsonConvert.DeserializeObject<ActivityResponse>(Settings.Activity);
            Title = $"Actividad en {activity.Neighborhood}";

        }
    }
}
