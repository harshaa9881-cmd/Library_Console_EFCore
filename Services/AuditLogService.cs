
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Services
{
    public class AuditLogService
    {
        private readonly UnitOfWork _unitOfWork;

        public AuditLogService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            return await _unitOfWork.AuditLogRepository.GetAllAsync();
        }

        public async Task AddAsync(string action, string description)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("Action cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty.");
            }

            AuditLog auditLog = new AuditLog
            {
                Action = action,
                Description = description,
                ActionDate = DateTime.Now
            };

            await _unitOfWork.AuditLogRepository.AddAsync(auditLog);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
