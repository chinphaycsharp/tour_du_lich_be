using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class images
    {
        public int id { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public int typeId { get; set; }
    }
}
