
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
            return await _context.Members.ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int memberId)
        {
            return await _context.Members.FindAsync(memberId);
        }

        public async Task AddAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Member member)
        {
            var existingMember = await _context.Members
                .FindAsync(member.MemberId);

            if (existingMember == null)
            {
                return false;
            }

            existingMember.MemberName = member.MemberName;
            existingMember.Email = member.Email;
            existingMember.PhoneNumber = member.PhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);

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
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
