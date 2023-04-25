using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPS.Data.Entities
{
    public partial class tour_connect_hotel
    {
        public int id_tour { get; set; }
        public int id_hotel { get; set; }

        [ForeignKey("id_tour")]
        [InverseProperty("tour_connect_hotels")]
        public virtual tour tour { get; set; }

        [ForeignKey("id_hotel")]
        [InverseProperty("tour_connect_hotels")]
        public virtual hotel hotel { get; set; }
    }
}
