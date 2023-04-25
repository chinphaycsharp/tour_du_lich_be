using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.API.Commons
{
    public class ExcelHelper
    {
        public static ExcelPackage CreateDoc(string title, string subject, string keyword)
        {
            var p = new ExcelPackage();
            p.Workbook.Properties.Title = title;
            p.Workbook.Properties.Author = "Application Name";
            p.Workbook.Properties.Subject = subject;
            p.Workbook.Properties.Keywords = keyword;
            return p;
        }
    }
}
