using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    //Creamos la entidad Tipo de servicio
    public class ActivityType
    {
        //Declaramos la propiedad id de la clase
        public int Id { get; set; }
        //Mostramos el nombre del campo en pantalla
        [Display(Name = "Activity Type")]
        //Tamaño máximo de caracteres
        [MaxLength(50, ErrorMessage = "The {0} Field cann't have more than {1} characters.")]
        //El campo es obligatorio
        [Required(ErrorMessage = "That field {0} is required.")]

        //Declaramos la propiedad nombre
        public string Name { get; set; }
        
        public ICollection<Activity> Activities { get; set; }

    }
}

