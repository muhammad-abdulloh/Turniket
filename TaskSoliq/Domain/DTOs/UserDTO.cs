using System.ComponentModel.DataAnnotations.Schema;
using TaskSoliq.Domain.Enums;

namespace TaskSoliq.Domain.DTOs
{
    /// <summary>
    /// input dates
    /// </summary>
    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public EmployeeCategory? EmployeeCategory { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
