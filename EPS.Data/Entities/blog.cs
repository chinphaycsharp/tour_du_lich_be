﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Data.Entities
{
    public partial class blog
    {
        public int id { get; set; }
        public string title { get; set; }
        public string img_src { get; set; }
        public string content { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
    }
}