using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.ImageBlog
{
    public class ImageBlogCreateDto
    {
        public int id { get; set; }
        public int id_blog { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
        public ImageBlogCreateDto(int IdBlog, string ImgSrc)
        {
            this.id_blog = IdBlog;
            this.img_src = ImgSrc;
            this.status = 1;
        }
    }
}
