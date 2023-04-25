using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Common
{
    public class ImageToBase64
    {
        public static string HandleImage(string fileName,string folder)
        {
            //string file = Directory.GetFile("wwwroot/images/" + fileName);
            //string result = "";
            //if (fileName != null)
            //{
            //    string path = "wwwroot/images/" + fileName;
            //    byte[] imageArray = System.IO.File.ReadAllBytes(path);
            //    result = Convert.ToBase64String(imageArray);
            //}

            //return result;
            string result = "http://localhost:1938/"+ folder+"/" + fileName;
            return result;
        }
    }
}
