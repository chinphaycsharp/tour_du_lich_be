using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.RolePrivilege
{
    public class RolePrivilegeUpdateDto
    {
        public int id { get; set; }
        public string[] privileges { get; set; }
    }
}
