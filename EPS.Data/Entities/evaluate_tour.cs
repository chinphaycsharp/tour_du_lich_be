using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class evaluate_tour
    {
        public int id { get; set; }
        public int id_tour { get; set; }

        [ForeignKey("id_tour")]
        [InverseProperty("evaluate_tours")]
        public virtual tour tour { get; set; }
        public string content { get; set; }
        public int star_count { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
    }
}
