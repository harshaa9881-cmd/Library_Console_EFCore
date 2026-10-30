using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagementSystem.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
    }
}
