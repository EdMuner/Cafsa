using Cafsa.Common.Models;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace Cafsa.Prism.ViewModels
{
    public class ActivitiesPageViewModel : ViewModelBase
    {
        private RefereeResponse _referee;
        private ObservableCollection<ActivityResponse> _activities;

        public ActivitiesPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Activities";
        }

        public ObservableCollection<ActivityResponse> Activities
        {
            get => _activities;
            set => SetProperty(ref _activities, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("referee"))
            {
                //se trae la informacion de las activities del referee especifico
                _referee = parameters.GetValue<RefereeResponse>("referee");
                Title = $"Activities of: {_referee.FullName}";
                Activities = new ObservableCollection<ActivityResponse>(_referee.Activities);

            }
        }

    }
}
