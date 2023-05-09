
using EPS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Service.Dtos.Hotel
{
    public class HotelGridDto
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string updated_timeStr { get; set; }
        public string created_timeStr { get; set; }
        public int status { get; set; }
    }
}
