using System;
using System.Collections.Generic;
using System.Text;

namespace Cafsa.Common.Models
{
    public class ImageRequest
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public byte[] ImageArray { get; set; }

    }

}
