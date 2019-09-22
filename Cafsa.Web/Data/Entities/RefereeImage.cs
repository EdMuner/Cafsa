using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    public class RefereeImage
    {
        public int Id { get; set; }



        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


       
        public string ImageFullPath => String.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://myleasingmunera.azurewebsites.net{ImageUrl.Substring(1)}";

    }
}
