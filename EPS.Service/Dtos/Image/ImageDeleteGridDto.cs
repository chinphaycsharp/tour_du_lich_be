using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Image
{
    public class ImageDeleteGridDto
    {
        public List<int> ids { get; set; }
        public List<string> names{ get; set; }
    }
}
