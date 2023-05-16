using EPS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPS.Service.Dtos.ImageBlog
{
    public class ImageBlogGridDto
    {
        public int id { get; set; }
        public int id_blog { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
    }
}
