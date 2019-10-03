using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Data.Entities
{
    public class Referee : User
    {
       

        public User User { get; set; }

       public RefereeType RefereeType { get; set; }


        public ICollection<Service> Services { get; set; }

        public ICollection<Contract> Contracts { get; set; }
     

    }
}
