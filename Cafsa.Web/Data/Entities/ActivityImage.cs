﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data.Entities
{
    public class ActivityImage
    {
        public int Id { get; set; }


        //imagen del campo de juego
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        // TODO: Change the path when publish
        public string ImageFullPath => String.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://cafsa.azurewebsites.net{ImageUrl.Substring(1)}";

        public Activity Activity { get; set; }

    }
}
