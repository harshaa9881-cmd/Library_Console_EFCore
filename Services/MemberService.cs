
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Services
{
    public class MemberService
    {
        private readonly UnitOfWork _unitOfWork;

        public MemberService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _unitOfWork.MemberRepository.GetAllAsync();
        }

        public async Task<Member?> GetByIdAsync(int memberId)
        {
            return await _unitOfWork.MemberRepository.GetByIdAsync(memberId);
        }

        public async Task AddAsync(Member member)
        {
            ValidateMember(member);

            await _unitOfWork.MemberRepository.AddAsync(member);

            var auditLog = new AuditLog
            {
                Action = "Add Member",
                Description = $"Member '{member.MemberName}' added.",
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Member member)
        {
            ValidateMember(member);

            bool result = await _unitOfWork.MemberRepository.UpdateAsync(member);

            if (!result)
            {
                return false;
            }

            var auditLog = new AuditLog
            {
                Action = "Update Member",
                Description = $"Member ID {member.MemberId} updated.",
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int memberId)
        {
            bool result = await _unitOfWork.MemberRepository.DeleteAsync(memberId);

            if (!result)
            {
                return false;
            }

            var auditLog = new AuditLog
            {
                Action = "Delete Member",
                Description = $"Member ID {memberId} deleted.",
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();

            return true;
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
