using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Hotel
{
    public class HotelUpdateDto
    {
        public int category_id { get; set; }
        public string name { get; set; }
        public DateTime updated_time { get; set; }
        public string background_image { get; set; }
        public int status { get; set; }
    }
}
