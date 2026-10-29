using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementSystem.Models
{
    public class BookIssue
    {
        public int BookIssueId { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

