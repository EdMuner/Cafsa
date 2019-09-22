using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }

        public int Price { get; set; }


        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; }

    }
}
