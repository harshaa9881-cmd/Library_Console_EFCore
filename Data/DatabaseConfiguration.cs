using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementSystem.Data
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } =
            "Server=(local)\\SQLEXPRESS;Database=LibraryManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
