using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Image
{
    public class ImageGridDto
    {
        public int id { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public int typeId { get; set; }
    }
}
