using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPS.Service.Dtos.Password
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string userName { get; set; }
        public bool EmailSent { get; set; }
        public string newPassword { get; set; }
        public Nullable<DateTime> DateValidateToken { get; set; }
    }
}
