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
        [Description("Xem người dùng")]
        ViewUser,
        [Description("Sửa người dùng")]
        ManageUser,
        [Description("Xem nhóm người dùng")]
        ViewRole,
        [Description("Sửa nhóm người dùng")]
        ManageRole,
        [Description("Xem danh mục")]
        ViewCategory,
        [Description("Sửa danh mục")]
        EditCategory,
        [Description("Thêm mới danh mục")]
        CreateCategory,
        [Description("Xoá danh mục")]
        DeleteCategory,
        [Description("Xem cây")]
        ViewTree,
        [Description("Sửa cây")]
        EditTree,
        [Description("Thêm cây")]
        CreateTree,
        [Description("Xoá cây")]
        DeleteTree,
        [Description("Xem danh mục bài viết")]
        ViewTag,
        [Description("Sửa danh mục bài viết")]
        EditTag,
        [Description("Tạo mới mục bài viết")]
        CreateTag,
        [Description("Xóa mục bài viết")]
        DeleteTag,
        [Description("Xem đơn hàng")]
        ViewOrder,
        [Description("Sửa Đơn hàng")]
        ConfirmOrder,
        [Description("Tạo mới đơn hàng")]
        CreateOrder,
        [Description("Xóa đơn hàng")]
        DeleteOrder,
        [Description("Xác nhận đơn hàng")]
        OrderConfirm,
        [Description("Xem dụng cụ")]
        ViewTool,
        [Description("Sửa dụng cụ")]
        EditTool,
        [Description("Tạo mới dụng cụ")]
        CreateTool,
        [Description("Xóa dụng cụ")]
        DeleteTool,
        [Description("Xem bài viết")]
        ViewPost,
        [Description("Sửa bài viết")]
        EditPost,
        [Description("Tạo bài viết")]
        CreatePost,
        [Description("Xóa bài viết")]
        DeletePost,
        [Description("Xem ảnh")]
        ViewImage,
        [Description("Sửa ảnh")]
        EditImage,
        [Description("Upload ảnh")]
        UploadImage,
        [Description("Xóa ảnh")]
        DeleteImage
    }
}
