using System;
using System.Collections.Generic;
using System.Text;

namespace Cafsa.Common.Models
{
    public class RefereeResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Document { get; set; }

        public string Address { get; set; }

        public string Category { get; set; }

      

        public ICollection<ActivityResponse> Activities { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
