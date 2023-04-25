using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Chat
{
    public class ChatGridDto
    {
        public string clientuniqueid { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
    }
}
