using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Menus
{
    public class BookMenu
    {
        private readonly BookService _bookService;

        public BookMenu(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("\n--- Book Menu ---");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Search Book");
                Console.WriteLine("4. View Book By ID");
                Console.WriteLine("5. Update Book");
                Console.WriteLine("6. Delete Book");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await AddBookAsync();
                            break;

                        case "2":
                            await ViewAllBooksAsync();
                            break;

                        case "3":
                            await SearchBookAsync();
                            break;

                        case "4":
                            await ViewBookByIdAsync();
                            break;

                        case "5":
                            await UpdateBookAsync();
                            break;

                        case "6":
                            await DeleteBookAsync();
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Operation failed: {ex.Message}");
                }
            }
        }

        private async Task AddBookAsync()
        {
            var book = new Book();

            Console.Write("Enter title: ");
            book.Title = Console.ReadLine() ?? "";

            Console.Write("Enter author: ");
            book.Author = Console.ReadLine() ?? "";

            Console.Write("Enter ISBN: ");
            book.ISBN = Console.ReadLine() ?? "";

            Console.Write("Enter category: ");
            book.Category = Console.ReadLine() ?? "";

            book.TotalCopies = ReadInt("Enter total copies: ");
            book.AvailableCopies = book.TotalCopies;

            await _bookService.AddAsync(book);

            Console.WriteLine("Book added successfully.");
        }

        private async Task ViewAllBooksAsync()
        {
            var books = await _bookService.GetAllAsync();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            Console.WriteLine(
                "\n{0,-8} {1,-25} {2,-20} {3,-18} {4,-15} {5,-10} {6,-10}",
                "ID", "Title", "Author", "ISBN",
                "Category", "Total", "Available");

            foreach (var book in books)
            {
                Console.WriteLine(
                    "{0,-8} {1,-25} {2,-20} {3,-18} {4,-15} {5,-10} {6,-10}",
                    book.BookId,
                    book.Title,
                    book.Author,
                    book.ISBN,
                    book.Category,
                    book.TotalCopies,
                    book.AvailableCopies);
            }
        }

        private async Task SearchBookAsync()
        {
            Console.Write("Enter title or author to search: ");
            string searchText = Console.ReadLine() ?? "";

            var books = await _bookService.SearchAsync(searchText);

            if (books.Count == 0)
            {
                Console.WriteLine("No matching books found.");
                return;
            }

            foreach (var book in books)
            {
                Console.WriteLine(
                    $"ID: {book.BookId}, Title: {book.Title}, " +
                    $"Author: {book.Author}, Available: {book.AvailableCopies}");
            }
        }

        private async Task ViewBookByIdAsync()
        {
            int bookId = ReadInt("Enter book ID: ");

            var book = await _bookService.GetByIdAsync(bookId);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            DisplayBook(book);
        }

        private async Task UpdateBookAsync()
        {
            int bookId = ReadInt("Enter book ID to update: ");

            var book = await _bookService.GetByIdAsync(bookId);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.Write("Enter new title: ");
            book.Title = Console.ReadLine() ?? "";

            Console.Write("Enter new author: ");
            book.Author = Console.ReadLine() ?? "";

            Console.Write("Enter new ISBN: ");
            book.ISBN = Console.ReadLine() ?? "";

            Console.Write("Enter new category: ");
            book.Category = Console.ReadLine() ?? "";

            int totalCopies = ReadInt("Enter new total copies: ");

            int issuedCopies = book.TotalCopies - book.AvailableCopies;

            if (totalCopies < issuedCopies)
            {
                Console.WriteLine(
                    $"Total copies cannot be less than issued copies ({issuedCopies}).");
                return;
            }

            book.TotalCopies = totalCopies;
            book.AvailableCopies = totalCopies - issuedCopies;

            bool updated = await _bookService.UpdateAsync(book);

            Console.WriteLine(updated
                ? "Book updated successfully."
                : "Book could not be updated.");
        }

        private async Task DeleteBookAsync()
        {
            int bookId = ReadInt("Enter book ID to delete: ");

            bool deleted = await _bookService.DeleteAsync(bookId);

            Console.WriteLine(deleted
                ? "Book deleted successfully."
                : "Book not found or has issue history. It cannot be deleted.");
        }

        private void DisplayBook(Book book)
        {
            Console.WriteLine($"Book ID: {book.BookId}");
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"Author: {book.Author}");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine($"Category: {book.Category}");
            Console.WriteLine($"Total copies: {book.TotalCopies}");
            Console.WriteLine($"Available copies: {book.AvailableCopies}");
        }

        private int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }

                Console.WriteLine("Please enter a valid number.");
            }
        }
    }
}
