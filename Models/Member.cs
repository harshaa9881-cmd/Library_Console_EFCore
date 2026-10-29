using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementSystem.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}

