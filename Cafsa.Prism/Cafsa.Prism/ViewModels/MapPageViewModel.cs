using Cafsa.Prism.Helpers;
using Prism.Navigation;

namespace Cafsa.Prism.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        public MapPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Map;
        }
    }
}
