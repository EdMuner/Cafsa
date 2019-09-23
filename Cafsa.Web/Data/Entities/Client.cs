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
        public string Document { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Address { get; internal set; }
        public string Phone { get; internal set; }
    }
}

