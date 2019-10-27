
using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Models
{
    public class ActivityViewModel : Activity
    {
        public int RefereeId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Activity Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Activity type.")]
        public int ActivityTypeId { get; set; }

       

        public IEnumerable<SelectListItem> ActivityTypes { get; set; }

       


    }
}
