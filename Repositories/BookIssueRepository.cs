using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class BookIssueRepository
    {
        private readonly LibraryDbContext _context;

        public BookIssueRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookIssue>> GetAllAsync()
        {
            return await _context.BookIssues.ToListAsync();
        }

        public async Task<bool> IssueBookAsync(
            int bookId, int memberId, DateTime dueDate)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return false;
            }

            var member = await _context.Members.FindAsync(memberId);

            if (member == null)
            {
                return false;
            }

            if (book.AvailableCopies <= 0)
            {
                return false;
            }

            var bookIssue = new BookIssue
            {
                BookId = bookId,
                MemberId = memberId,
                IssueDate = DateTime.Today,
                DueDate = dueDate,
                ReturnDate = null
            };

            book.AvailableCopies--;

            _context.BookIssues.Add(bookIssue);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReturnBookAsync(int bookIssueId)
        {
            var bookIssue = await _context.BookIssues
                .FindAsync(bookIssueId);

            if (bookIssue == null)
            {
                return false;
            }

            if (bookIssue.ReturnDate != null)
            {
                return false;
            }

            var book = await _context.Books.FindAsync(bookIssue.BookId);

            if (book == null)
            {
                return false;
            }

            bookIssue.ReturnDate = DateTime.Today;

            if (book.AvailableCopies < book.TotalCopies)
            {
                book.AvailableCopies++;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
