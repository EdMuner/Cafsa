using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Models
{
    public class ContractViewModel : Contract
    {
        //se conecta con el Id del Referee que tiene el contract
            
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Client")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Client.")]
        public int ClientId { get; set; }

        public IEnumerable<SelectListItem> Clients { get; set; }
    }
}
