using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cafsa.Common.Models
{
    public class ActivityRequest
    {
        public int Id { get; set; }

        [Required]
        public string Neighborhood { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string Remarks { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }

        [Required]
        public int RefereeId { get; set; }
    }

}
