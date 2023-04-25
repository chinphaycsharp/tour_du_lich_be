using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class register_tour
    {
        public int id { get; set; }
        public int id_tour { get; set; }

        [ForeignKey("id_tour")]
        [InverseProperty("register_tours")]
        public virtual tour tour { get; set; }
        public string name_register { get; set; }
        public string address_register { get; set; }
        public string phone_register { get; set; }
        public string email_register { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
    }
}
