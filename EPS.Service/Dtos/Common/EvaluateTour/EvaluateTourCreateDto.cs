using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Common.EvaluateTour
{
    public class EvaluateTourCreateDto
    {
        public int id { get; set; }
        public int id_tour { get; set; }
        public string content { get; set; }
        public int star_count { get; set; }
        public DateTime created_time { get; set; }
        public int status { get; set; }
    }
}
