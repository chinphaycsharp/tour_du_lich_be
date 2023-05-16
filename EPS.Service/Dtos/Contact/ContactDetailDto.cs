using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Contact
{
    public class ContactDetailDto
    {
        public int id { get; set; }
        public string name_register { get; set; }
        public string address_register { get; set; }
        public string phone_register { get; set; }
        public string email_register { get; set; }
        public string created_timeStr { get; set; }
        public string updated_timeStr { get; set; }
        public int status { get; set; }
    }
}
