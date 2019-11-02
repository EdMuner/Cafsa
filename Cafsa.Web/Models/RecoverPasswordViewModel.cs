using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
