using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Tour
{
    public class TourCreateDto
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime created_time { get; set; }
        public int status { get; set; }
        public string background_image { get; set; }
        public TourCreateDto(int CategoryId, string Name, string Url, string BackgroundImage)
        {
            category_id = CategoryId;
            name = Name;
            url = Url;
            status = 1;
            background_image = BackgroundImage;
        }
    }
}
