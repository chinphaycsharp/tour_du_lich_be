using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPS.Data.Entities
{
    public partial class category
    {
        public category()
        {
            hotels = new HashSet<hotel>();
            tours = new HashSet<tour>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }

        public virtual ICollection<hotel> hotels { get; set; }
        public virtual ICollection<tour> tours { get; set; }
    }
}
