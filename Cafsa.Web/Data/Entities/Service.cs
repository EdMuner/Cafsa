﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; }


        public ServiceType ServiceType { get; set; } 

        public ICollection<Contract> Contracts { get; set; }

        public ICollection<ServiceImage> ServiceImages { get; set; }

    }
}
