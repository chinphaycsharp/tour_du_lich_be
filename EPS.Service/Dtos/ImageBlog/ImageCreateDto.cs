using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.ImageBlog
{
    public class ImageCreateDto
    {
        public int id { get; set; }
        public int type_id { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string type { get; set; }
        public ImageCreateDto(int IdType, string ImgSrc, string Type)
        {
            type_id = IdType;
            img_src = ImgSrc;
            status = 1;
            created_time = DateTime.Now;
            type = Type;
        }
    }
}
