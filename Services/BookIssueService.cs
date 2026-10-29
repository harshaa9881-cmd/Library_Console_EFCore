using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class BookIssueService
    {
        private readonly BookIssueRepository _bookIssueRepository;

        public BookIssueService(BookIssueRepository bookIssueRepository)
        {
            _bookIssueRepository = bookIssueRepository;
        }

        public async Task<List<BookIssue>> GetAllAsync()
        {
            return await _bookIssueRepository.GetAllAsync();
        }

        public async Task<bool> IssueBookAsync(
            int bookId, int memberId, DateTime dueDate)
        {
            if (bookId <= 0)
            {
                throw new ArgumentException("Enter a valid book ID.");
            }

            if (memberId <= 0)
            {
                throw new ArgumentException("Enter a valid member ID.");
            }

            if (dueDate.Date <= DateTime.Today)
            {
                throw new ArgumentException(
                    "Due date must be after today.");
            }

            return await _bookIssueRepository.IssueBookAsync(
                bookId, memberId, dueDate);
        }

        public async Task<bool> ReturnBookAsync(int bookIssueId)
        {
            if (bookIssueId <= 0)
            {
                throw new ArgumentException(
                    "Enter a valid book issue ID.");
            }

            return await _bookIssueRepository.ReturnBookAsync(bookIssueId);
        }
    }
}

