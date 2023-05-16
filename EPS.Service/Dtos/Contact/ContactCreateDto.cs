using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Contact
{
    public class ContactCreateDto
    {
        public int id { get; set; }
        public string name_register { get; set; }
        public string address_register { get; set; }
        public string phone_register { get; set; }
        public string email_register { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public int status { get; set; }
    }
}
