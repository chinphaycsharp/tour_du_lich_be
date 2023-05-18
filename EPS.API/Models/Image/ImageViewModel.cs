namespace EPS.API.Models.Image
{
    public class ImageViewModel
    {
        public int id { get; set; }
        public int type_id { get; set; }
        public string img_src { get; set; }
        public int status { get; set; }
        public string created_atStr { get; set; }
        public string updated_atStr { get; set; }
        public string type { get; set; }
    }
}
