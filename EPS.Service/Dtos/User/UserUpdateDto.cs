using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdministrator { get; set; }
        public int UnitId { get; set; }
        public string RoleIds { get; set; }
        public string NewPassword { get; set; }
        public DateTime CreateDate { get; set; }
        public string backgroundImage { get; set; }
        public string imageName { get; set; }
        public string? Address { get; set; }
    }
}
