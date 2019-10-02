using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    public class RefereeType
    {
        public int Id { get; set; }

        [Display(Name = "Referee Type")]
        [MaxLength(50, ErrorMessage = "The {0} Field cann't have more than {1} characters.")]
        [Required(ErrorMessage = "That field {0} is required.")]
        public string Name { get; set; }

        public ICollection<Referee> Referees { get; set; }
    }
}
