using Cafsa.Common.Models;
using Prism.Navigation;

namespace Cafsa.Prism.ViewModels
{
    public class ActivitiesPageViewModel : ViewModelBase
    {
        private RefereeResponse _referee;

        public ActivitiesPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Activities";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("referee"))
            {
                //se trae la informacion de las activities del referee especifico
                _referee = parameters.GetValue<RefereeResponse>("referee");
                Title = $"Activities of: {_referee.FullName}";


            }
        }

    }
}
