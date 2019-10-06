using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cafsa.Web.Models
{
    public class ServiceImageViewModel : ServiceImage
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
