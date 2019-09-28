using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Models
{
    public class EditRefereeViewModel : EditUserViewModel
    {
        [Display(Name = "Category")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Category { get; set; }

    }
}
