using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Category
{
    public class CategoryUpdateDto
    {
        public string name { get; set; }
        public string url { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
    }
}
