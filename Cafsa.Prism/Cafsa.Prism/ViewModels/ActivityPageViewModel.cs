using Cafsa.Common.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cafsa.Prism.ViewModels
{
    public class ActivityPageViewModel : ViewModelBase
    {


        private ActivityResponse _activity;
        private ObservableCollection<RotatorModel> _imageCollection;

        public ActivityPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Activity";
        }
        //este metodo sirve para que cuando yo navegue capturar la actividad y la bindamos para traer todos los campos referentes
        public ActivityResponse Activity
        {
            get => _activity;
            set => SetProperty(ref _activity, value);

        }
        public ObservableCollection<RotatorModel> ImageCollection
        {
            get => _imageCollection;
            set => SetProperty(ref _imageCollection, value);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)

        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("activity"))
            {
                Activity = parameters.GetValue<ActivityResponse>("activity");
                Title = $"Activity: { Activity.Neighborhood}";
                LoadImages();

            }
        }

        private void LoadImages()
        {
            var list = new List<RotatorModel>();
            foreach (var activityImage in Activity.ActivityImages)
            {
                list.Add(new RotatorModel { Image = activityImage.ImageUrl });
            }

            ImageCollection = new ObservableCollection<RotatorModel>(list);
        }
    }

}
