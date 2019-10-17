using System.Collections.Generic;
using System.Linq;


namespace Cafsa.Common.Models
{
   public class ActivityResponse
    {
        public int Id { get; set; }

        public string Neighborhood { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string Remarks { get; set; }

        public string ActivityType { get; set; }

        public ICollection<ActivityImageResponse> ActivityImages { get; set; }

        public ICollection<ServiceResponse> Services { get; set; }

        public string FirstImage => ActivityImages == null || ActivityImages.Count == 0
                ? "https://cafsa.azurewebsites.net/images/Activities/noImage.png"
                : ActivityImages.FirstOrDefault().ImageUrl;

    }
}
