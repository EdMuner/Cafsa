using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    public class RefereeImage
    {
        public int Id { get; set; }

        [Display(Name = "Referee Pic")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string ImageUrl { get; set; }

        public string ImageFullPath => $"https://TBD.azurewebsites.net{ImageUrl.Substring(1)}";
    }
}
