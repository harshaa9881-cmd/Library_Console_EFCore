
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class MemberRepository
    {
        private readonly LibraryDbContext _context;

        public MemberRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members
                .ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int memberId)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == memberId);
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
        }

        public async Task<bool> UpdateAsync(Member member)
        {
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == member.MemberId);

            if (existingMember == null)
            {
                return false;
            }

            existingMember.MemberName = member.MemberName;
            existingMember.Email = member.Email;
            existingMember.PhoneNumber = member.PhoneNumber;

            return true;
        }

        public async Task<bool> DeleteAsync(int memberId)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == memberId);

            if (member == null)
            {
                return false;
            }

            bool hasIssueHistory = await _context.BookIssues
                .AnyAsync(bi => bi.MemberId == memberId);

            if (hasIssueHistory)
            {
                return false;
            }

            _context.Members.Remove(member);

            return true;
        }
    }
}
