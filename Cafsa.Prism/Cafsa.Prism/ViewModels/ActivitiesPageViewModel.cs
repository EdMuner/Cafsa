using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Cafsa.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class ActivitiesPageViewModel : ViewModelBase
    {
        private RefereeResponse _referee;
        private ObservableCollection<ActivityItemViewModel> _activities;
        private INavigationService _navigationService;
        private DelegateCommand _addActivityCommand;
   

        public ActivitiesPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.Activities;
            LoadReferee();
        }

        public ObservableCollection<ActivityItemViewModel> Activities
        {
            get => _activities;
            set => SetProperty(ref _activities, value);
        }

        public DelegateCommand AddActivityCommand => _addActivityCommand ?? (_addActivityCommand = new DelegateCommand(AddActivityAsync));

      

        private void LoadReferee()

        {  //se trae la informacion de las activities del referee especifico
            _referee = JsonConvert.DeserializeObject<RefereeResponse>(Settings.Referee);
            if (_referee.RoleId == 1)
            {
                Title = $"Actividades {_referee.FullName}";
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
            else
            {
                Title = Languages.Activities;
                Activities = new ObservableCollection<ActivityItemViewModel>(_referee.Activities.Select(a => new ActivityItemViewModel(_navigationService)
                {
                    Address = a.Address,
                    Services = a.Services,
                    IsAvailable = a.IsAvailable,
                    Id = a.Id,
                    Neighborhood = a.Neighborhood,
                    //Price = a.Price,
                    ActivityImages = a.ActivityImages,
                    ActivityType = a.ActivityType,
                    Remarks = a.Remarks

                }).ToList()); 
            }
        }

        private async void AddActivityAsync()
        {
            await _navigationService.NavigateAsync("EditActivityPage");
        }
    }
}
