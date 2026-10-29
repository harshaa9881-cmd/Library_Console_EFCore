
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class MemberService
    {
        private readonly MemberRepository _memberRepository;

        public MemberService(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetByIdAsync(int memberId)
        {
            return await _memberRepository.GetByIdAsync(memberId);
        }

        public async Task AddAsync(Member member)
        {
            ValidateMember(member);
            await _memberRepository.AddAsync(member);
        }

        public async Task<bool> UpdateAsync(Member member)
        {
            ValidateMember(member);
            return await _memberRepository.UpdateAsync(member);
        }

        public async Task<bool> DeleteAsync(int memberId)
        {
            return await _memberRepository.DeleteAsync(memberId);
        }

        private void ValidateMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentException("Member details are required.");
            }

            if (string.IsNullOrWhiteSpace(member.MemberName))
            {
                throw new ArgumentException("Member name is required.");
            }

            if (string.IsNullOrWhiteSpace(member.Email) ||
                !member.Email.Contains('@'))
            {
                throw new ArgumentException("Enter a valid email address.");
            }

            if (string.IsNullOrWhiteSpace(member.PhoneNumber))
            {
                throw new ArgumentException("Phone number is required.");
            }
        }
    }
}
