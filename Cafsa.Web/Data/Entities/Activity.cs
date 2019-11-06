using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cafsa.Web.Data.Entities
{
    public class Activity
    {
        internal static readonly string Current;

        public int Id { get; set; }

        [Display(Name = "Neighborhood")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Neighborhood { get; set; }

        [Display(Name = "Address")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Address { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        public ActivityType ActivityType { get; set; }

        public Referee Referee { get; set; }

        public ICollection<ActivityImage> ActivityImages { get; set; }

        public ICollection<Service> Services { get; set; }

        public string FirstImage => ActivityImages == null || ActivityImages.Count == 0
              ? "https://cafsa.azurewebsites.net/images/Services/noImage.png"
              : ActivityImages.FirstOrDefault().ImageUrl;


    }
}
