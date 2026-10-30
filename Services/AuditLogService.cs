
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class AuditLogService
    {
        private readonly AuditLogRepository _repository;

        public AuditLogService(AuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(string action, string description)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                throw new Exception("Action cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("Description cannot be empty.");
            }

            AuditLog auditLog = new AuditLog
            {
                Action = action,
                Description = description,
                ActionDate = DateTime.Now
            };

            await _repository.AddAsync(auditLog);
        }
    }
}
