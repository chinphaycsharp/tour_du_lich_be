using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class image_hotel
    {
        public int id { get; set; }
        public int id_hotel { get; set; }

        [ForeignKey("id_hotel")]
        [InverseProperty("image_hotels")]
        public virtual hotel hotel { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
    }
}
