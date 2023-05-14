using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class image_blog
    {
        public int id { get; set; }
        public int id_blog { get; set; }

        [ForeignKey("id_blog")]
        [InverseProperty("image_blogs")]
        public virtual blog blog{ get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
    }
}
