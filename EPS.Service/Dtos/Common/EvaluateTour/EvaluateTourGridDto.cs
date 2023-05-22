using EPS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Service.Dtos.Common.EvaluateTour
{
    public class EvaluateTourGridDto
    {
        public int id { get; set; }
        public int id_tour { get; set; }
        public string content { get; set; }
        public int star_count { get; set; }
        public DateTime created_time { get; set; }
        public string created_timeStr { get; set; }
        public DateTime updated_time { get; set; }
        public string updated_timeStr { get; set; }
        public int status { get; set; }
    }
}
