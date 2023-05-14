﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Blog
{
    public class BlogUpdateDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string img_src { get; set; }
        public string content { get; set; }
        public DateTime updated_time { get; set; }
    }
}
