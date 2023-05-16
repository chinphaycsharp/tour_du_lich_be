using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Blog.BlogContent
{
    public class BlogContentCreateDto
    {
        public int id { get; set; }
        public int id_blog { get; set; }
        public string content { get; set; }
        public int status { get; set; }
        public int order { get; set; }
    }
}
