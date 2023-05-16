using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class blog
    {
        public blog()
        {
            image_blogs = new HashSet<image_blog>();
        }

        public int id { get; set; }
        public string title { get; set; }
        public string img_src { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public virtual ICollection<image_blog> image_blogs { get; set; }
    }
}
