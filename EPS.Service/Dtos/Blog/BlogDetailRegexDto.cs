﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Blog
{
    public class BlogDetailRegexDto
    {
        public List<string> contents { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string img_src { get; set; }
        public string created_timeStr { get; set; }
        public string updated_timeStr { get; set; }
    }
}
