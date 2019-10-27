using System;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Date - Hour")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Date - Hour")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDateLocal => StartDate.ToLocalTime();

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public Referee Referee { get; set; }

        public Client Client { get; set; }

        public Activity Activity { get; set; }
    }
}
        