using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Cafsa.Prism.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Cafsa.Prism.Views
{
    public partial class ActivityPage : ContentPage
    {
        public ActivityPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            rotator.ItemsSource = GetDataSource();

            var imageTemplate = new DataTemplate(() =>
            {
                var image = new Image();
                image.SetBinding(Image.SourceProperty, "Image");
                return image;
            });

            rotator.ItemTemplate = imageTemplate;
        }

        private IEnumerable<CustomData> GetDataSource()
        {
            List<CustomData> list = new List<CustomData>();
            var propertyImages = JsonConvert.DeserializeObject<List<ActivityImageResponse>>(Settings.ActivityImages);
            foreach (var itepropertyImage in propertyImages)
            {
                list.Add(new CustomData(itepropertyImage.ImageUrl));
            }

            return list;
        }
    }

}
}
