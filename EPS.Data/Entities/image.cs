using System;

namespace EPS.Data.Entities
{
    public partial class image
    {
        public int id { get; set; }
        public int type_id { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string type { get; set; }
    }
}
