using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.Privilege
{
    public class PrivilegeCreateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
    }
}
