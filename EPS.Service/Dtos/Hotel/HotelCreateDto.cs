using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Hotel
{
    public class HotelCreateDto
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string background_image { get; set; }
        public DateTime created_time { get; set; }
        public int status { get; set; }
    }
}
