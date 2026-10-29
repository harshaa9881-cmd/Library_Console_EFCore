
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        public BookService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetByIdAsync(int bookId)
        {
            return await _bookRepository.GetByIdAsync(bookId);
        }

        public async Task<List<Book>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentException("Search text cannot be empty.");
            }

            return await _bookRepository.SearchAsync(searchText);
        }

        public async Task AddAsync(Book book)
        {
            ValidateBook(book);

            if (await _bookRepository.IsISBNExistsAsync(book.ISBN))
            {
                throw new ArgumentException("This ISBN already exists.");
            }

            await _bookRepository.AddAsync(book);
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            ValidateBook(book);

            if (await _bookRepository.IsISBNExistsAsync(
                book.ISBN, book.BookId))
            {
                throw new ArgumentException("This ISBN already exists.");
            }

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<bool> DeleteAsync(int bookId)
        {
            return await _bookRepository.DeleteAsync(bookId);
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
