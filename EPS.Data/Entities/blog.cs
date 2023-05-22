using System;
using System.Collections.Generic;

namespace EPS.Data.Entities
{
    public partial class blog
    {
        public blog()
        {
        }

        public int id { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public string img_src { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}
