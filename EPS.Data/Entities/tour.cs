using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class tour
    {
        public tour()
        {
            evaluate_tours = new HashSet<evaluate_tour>();
            register_tours = new HashSet<register_tour>();
            tour_connect_hotels = new HashSet<tour_connect_hotel>();
        }
        public int id { get; set; }
        public int category_id { get; set; }

        [ForeignKey("category_id")]
        [InverseProperty("tours")]
        public virtual category category { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string background_image { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
        public virtual detail_tour detail_tour { get; set; }
        public virtual ICollection<evaluate_tour> evaluate_tours { get; set; }
        public virtual ICollection<register_tour> register_tours { get; set; }
        [InverseProperty("tour")]
        public virtual ICollection<tour_connect_hotel> tour_connect_hotels { get; set; }
    }
}
