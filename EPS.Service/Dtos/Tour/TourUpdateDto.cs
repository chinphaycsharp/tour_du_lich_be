using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Tour
{
    public class TourUpdateDto
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
    }
}
