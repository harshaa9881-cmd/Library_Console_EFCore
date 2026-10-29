using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Menus
{
    public class BookIssueMenu
    {
        private readonly BookIssueService _bookIssueService;

        public BookIssueMenu(BookIssueService bookIssueService)
        {
            _bookIssueService = bookIssueService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("\n--- Book Issue Menu ---");
                Console.WriteLine("1. Issue Book");
                Console.WriteLine("2. View All Issue Records");
                Console.WriteLine("3. Return Book");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await IssueBookAsync();
                            break;

                        case "2":
                            await ViewAllIssuesAsync();
                            break;

                        case "3":
                            await ReturnBookAsync();
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

        private async Task IssueBookAsync()
        {
            int bookId = ReadInt("Enter book ID: ");
            int memberId = ReadInt("Enter member ID: ");

            DateTime dueDate;

            while (true)
            {
                Console.Write("Enter due date (yyyy-MM-dd): ");

                if (DateTime.TryParse(
                    Console.ReadLine(), out dueDate))
                {
                    break;
                }

                Console.WriteLine("Enter a valid date.");
            }

            bool issued = await _bookIssueService.IssueBookAsync(
                bookId, memberId, dueDate);

            Console.WriteLine(issued
                ? "Book issued successfully."
                : "Book could not be issued. Check the book ID, member ID, and available copies.");
        }

        private async Task ViewAllIssuesAsync()
        {
            List<BookIssue> issues =
                await _bookIssueService.GetAllAsync();

            if (issues.Count == 0)
            {
                Console.WriteLine("No issue records found.");
                return;
            }

            Console.WriteLine(
                "\n{0,-12} {1,-10} {2,-12} {3,-15} {4,-15} {5,-15}",
                "Issue ID", "Book ID", "Member ID",
                "Issue Date", "Due Date", "Return Date");

            foreach (var issue in issues)
            {
                string returnDate = issue.ReturnDate.HasValue
                    ? issue.ReturnDate.Value.ToString("yyyy-MM-dd")
                    : "Not returned";

                Console.WriteLine(
                    "{0,-12} {1,-10} {2,-12} {3,-15} {4,-15} {5,-15}",
                    issue.BookIssueId,
                    issue.BookId,
                    issue.MemberId,
                    issue.IssueDate.ToString("yyyy-MM-dd"),
                    issue.DueDate.ToString("yyyy-MM-dd"),
                    returnDate);
            }
        }

        private async Task ReturnBookAsync()
        {
            int bookIssueId = ReadInt("Enter issue ID to return: ");

            bool returned =
                await _bookIssueService.ReturnBookAsync(bookIssueId);

            Console.WriteLine(returned
                ? "Book returned successfully."
                : "Return failed. Check the issue ID or whether the book was already returned.");
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

