
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private readonly UnitOfWork _unitOfWork;

        public BookService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _unitOfWork.BookRepository.GetAllAsync();
        }

        public async Task<Book?> GetByIdAsync(int bookId)
        {
            return await _unitOfWork.BookRepository.GetByIdAsync(bookId);
        }

        public async Task<List<Book>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentException("Search text cannot be empty.");
            }

            return await _unitOfWork.BookRepository.SearchAsync(searchText);
        }

        public async Task AddAsync(Book book)
        {
            ValidateBook(book);

            if (await _unitOfWork.BookRepository.IsISBNExistsAsync(book.ISBN))
            {
                throw new ArgumentException("This ISBN already exists.");
            }

            await _unitOfWork.BookRepository.AddAsync(book);

            var auditLog = new AuditLog
            {
                Action = "Add Book",
                Description = $"Book '{book.Title}' added.",
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            ValidateBook(book);

            if (await _unitOfWork.BookRepository.IsISBNExistsAsync(
                book.ISBN, book.BookId))
            {
                throw new ArgumentException("This ISBN already exists.");
            }

            bool result = await _unitOfWork.BookRepository.UpdateAsync(book);

            if (!result)
            {
                return false;
            }

            var auditLog = new AuditLog
            {
                Action = "Update Book",
                Description = $"Book ID {book.BookId} updated.",
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int bookId)
        {
            bool result = await _unitOfWork.BookRepository.DeleteAsync(bookId);

            if (!result)
            {
                return false;
            }

            var auditLog = new AuditLog
            {
                Action = "Delete Book",
                Description = $"Book ID {bookId} deleted.",
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private void ValidateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentException("Book details are required.");
            }

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new ArgumentException("Book title is required.");
            }

            if (string.IsNullOrWhiteSpace(book.Author))
            {
                throw new ArgumentException("Author name is required.");
            }

            if (string.IsNullOrWhiteSpace(book.ISBN))
            {
                throw new ArgumentException("ISBN is required.");
            }

            if (string.IsNullOrWhiteSpace(book.Category))
            {
                throw new ArgumentException("Book category is required.");
            }

            if (book.TotalCopies <= 0)
            {
                throw new ArgumentException(
                    "Total copies must be greater than zero.");
            }

            if (book.AvailableCopies < 0 ||
                book.AvailableCopies > book.TotalCopies)
            {
                throw new ArgumentException(
                    "Available copies must be between zero and total copies.");
            }
        }
    }
}
