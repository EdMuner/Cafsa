using Cafsa.Common.Models;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class ActivitiesPageViewModel : ViewModelBase
    {
        private RefereeResponse _referee;
        private ObservableCollection<ActivityItemViewModel> _activities;
        private INavigationService _navigationService;

        public ActivitiesPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Activities";
        }

        public ObservableCollection<ActivityItemViewModel> Activities
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
                Activities = new ObservableCollection<ActivityItemViewModel>(_referee.Activities.Select(a => new ActivityItemViewModel(_navigationService)
                {
                    Address = a.Address,
                    Services = a.Services,
                    IsAvailable = a.IsAvailable,
                    Id = a.Id,
                    Neighborhood = a.Neighborhood,
                    Price = a.Price,
                    ActivityImages = a.ActivityImages,
                    ActivityType = a.ActivityType,
                    Remarks = a.Remarks

                }).ToList());

            }
        }

    }
}
