using System;

namespace Cafsa.Common.Models
{
    public class ServiceResponse
    {
        public int Id { get; set; }

        public string Remarks { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }

        public ClientResponse Client { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

    }
}
