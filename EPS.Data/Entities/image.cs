using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class image
    {
        public int id { get; set; }
        public int type_id { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
    }
}
