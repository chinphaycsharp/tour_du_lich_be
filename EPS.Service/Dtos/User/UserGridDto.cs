using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Dtos.User
{
    public class UserGridDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsAdministrator { get; set; }
        public string PhoneNumber { get; set; }
        public string[] Roles { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int? DeletedUserId { get; set; }
        public string CreateDate { get; set; }
        public string backgroundImage { get; set; }
        public string? Address { get; set; }
    }
}
