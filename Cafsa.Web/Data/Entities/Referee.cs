using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Cafsa.Web.Data.Entities
{
    public class Referee
    {
        internal string Document;

        public int Id { get; set; }

        public User User { get; set; }

        [Display(Name = "Category")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Category { get; set; }

        public ICollection<RefereeImage> RefereeImages { get; set; }
      

        public ICollection<Contract> Contracts { get; set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Address { get; internal set; }
        public string Phone { get; internal set; }
    }
}
