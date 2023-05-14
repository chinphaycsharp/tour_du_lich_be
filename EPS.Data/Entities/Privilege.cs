using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPS.Data.Entities
{
    public partial class Privilege
    {
        public Privilege()
        {
            RolePrivileges = new HashSet<RolePrivilege>();
            UserPrivileges = new HashSet<UserPrivilege>();
        }

        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        public bool? Status { get; set; }

        [InverseProperty("Privilege")]
        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
        [InverseProperty("Privilege")]
        public virtual ICollection<UserPrivilege> UserPrivileges { get; set; }
    }

    public enum PrivilegeList
    {
        [Description("Quản lý người dùng")]
        ManageUser,
        [Description("Quản lý nhóm người dùng")]
        ManageRole,
        [Description("Quản lý tour")]
        ManageTour,
        [Description("Quản lý tour mục")]
        ManageCategory,
        [Description("Quản lý khách sạn")]
        ManageHotel,
        [Description("Quản lý ảnh")]
        ManageImage,
        [Description("Quản lý blog")]
        ManageBlog
    }
}
