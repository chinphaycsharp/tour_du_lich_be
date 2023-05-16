using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class content_blog
    {
        public int id { get; set; }
        public int id_blog { get; set; }
        public string content { get; set; }
        [ForeignKey("id_blog")]
        [InverseProperty("content_blogs")]
        public virtual blog blog{ get; set; }
        public int status { get; set; }
        public int order { get; set; }
    }
}
