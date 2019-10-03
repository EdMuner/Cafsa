using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Models
{
    public class RefereeViewModel : Referee
    {
        public int RefereeId { get; set; }
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Referee Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a property type.")]

        //codigo seleccionado en el combobox
        public int RefereeTypeId { get; set; }

        public IEnumerable<SelectListItem> RefereeTypes { get; set; }

    }
}
