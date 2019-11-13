using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Cafsa.Prism.Helpers;
using Newtonsoft.Json;
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
            Title = Languages.Details;
          
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
            Activity = JsonConvert.DeserializeObject<ActivityResponse>(Settings.Activity);
            LoadImages();
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
