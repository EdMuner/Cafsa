using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<Service> Services { get; set; }

       

    }
}

