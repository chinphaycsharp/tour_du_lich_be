using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.RolePrivilege
{
    public class RolePrivilegeGridDto
    {
        public int RoleId { get; set; }
        public string PrivilegeName { get; set; }
        public string PrivilegeId { get; set; }
        public bool? Status { get; set; }
    }
}
