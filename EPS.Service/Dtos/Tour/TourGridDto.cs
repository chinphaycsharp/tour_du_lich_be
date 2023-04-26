using System;

namespace EPS.Service.Dtos.Tour
{
    public class TourGridDto
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime created_time { get; set; }
        public string created_timeStr { get; set; }
        public DateTime updated_time { get; set; }
        public string updated_timeStr { get; set; }
        public int status { get; set; }
    }
}
