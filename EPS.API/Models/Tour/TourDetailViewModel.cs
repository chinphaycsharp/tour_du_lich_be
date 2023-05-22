using System;

namespace EPS.API.Models.Tour
{
    public class TourDetailViewModel
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime created_time { get; set; }
        public string created_timeStr { get; set; }
        public int status { get; set; }
        public string background_image { get; set; }
        public string price { get; set; }
        public string infor { get; set; }
        public string intro { get; set; }
        public string schedule { get; set; }
        public string policy { get; set; }
        public string note { get; set; }
        public string tour_guide { get; set; }
        public string isurance { get; set; }
        public TourDetailViewModel()
        {

        }

        public TourDetailViewModel(int id, int category_id, string name, string url, string created_timeStr, int status,
            string background_image, string price, string infor, string intro, string schedule, string policy, string note, string Tour_guide, string Isurance)
        {
            this.id = id;
            this.category_id = category_id;
            this.name = name;
            this.url = url;
            this.created_timeStr = created_timeStr;
            this.status = status;
            this.background_image = background_image;
            this.price = price;
            this.infor = infor;
            this.intro = intro;
            this.schedule = schedule;
            this.policy = policy;
            this.note = note;
            this.tour_guide = Tour_guide;
            this.isurance = Isurance;
        }
    }
}
