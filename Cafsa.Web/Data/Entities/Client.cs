using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public User User { get; set; }

        [Display(Name = "ClientName")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string ClientName { get; set; }

        public ICollection<Contract> Contracts { get; set; }
       
    }
}

