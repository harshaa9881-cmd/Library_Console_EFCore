using System;
using System.Collections.Generic;
using System.Text;
using Library_Console_EFCore.Repositories;
using LibraryManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagementSystem.Data
{
    public class UnitOfWork
    {
        private readonly LibraryDbContext _context;

        public BookRepository BookRepository { get; }
        public MemberRepository MemberRepository { get; }
        public BookIssueRepository BookIssueRepository { get; }
        public AuditLogRepository AuditLogRepository { get; }

        public UnitOfWork(
            LibraryDbContext context,
            BookRepository bookRepository,
            MemberRepository memberRepository,
            BookIssueRepository bookIssueRepository,
            AuditLogRepository auditLogRepository)
        {
            _context = context;
            BookRepository = bookRepository;
            MemberRepository = memberRepository;
            BookIssueRepository = bookIssueRepository;
            AuditLogRepository = auditLogRepository;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}