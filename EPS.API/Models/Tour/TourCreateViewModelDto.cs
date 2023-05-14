using System;

namespace EPS.API.Models.Tour
{
    public class TourCreateViewModelDto
    {
        public int category_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
        public string price { get; set; }
        public string infor { get; set; }
        public string intro { get; set; }
        public string background_image { get; set; }
        public string schedule { get; set; }
        public string policy { get; set; }
        public string note { get; set; }
    }
}
