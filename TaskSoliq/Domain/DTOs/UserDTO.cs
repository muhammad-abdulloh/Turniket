using TaskSoliq.Domain.Enums;

namespace TaskSoliq.Domain.DTOs
{
    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public EmployeeCategory? EmployeeCategory { get; set; }
    }
}
