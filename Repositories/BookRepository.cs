
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int bookId)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task<List<Book>> SearchAsync(string searchText)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchText) ||
                            b.Author.Contains(searchText))
                .ToListAsync();
        }

        public async Task<bool> IsISBNExistsAsync(string isbn, int bookId = 0)
        {
            return await _context.Books
                .AnyAsync(b => b.ISBN == isbn && b.BookId != bookId);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.BookId == book.BookId);

            if (existingBook == null)
            {
                return false;
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.ISBN = book.ISBN;
            existingBook.Category = book.Category;
            existingBook.TotalCopies = book.TotalCopies;
            existingBook.AvailableCopies = book.AvailableCopies;

            return true;
        }

        public async Task<bool> DeleteAsync(int bookId)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                return false;
            }

            bool hasIssueHistory = await _context.BookIssues
                .AnyAsync(bi => bi.BookId == bookId);

            if (hasIssueHistory)
            {
                return false;
            }

            _context.Books.Remove(book);

            return true;
        }
    }
}
