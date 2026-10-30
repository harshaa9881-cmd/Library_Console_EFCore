using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired();

            modelBuilder.Entity<BookIssue>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(bi => bi.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookIssue>()
                .HasOne<Member>()
                .WithMany()
                .HasForeignKey(bi => bi.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
