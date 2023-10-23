using System.ComponentModel.DataAnnotations;
using TaskSoliq.Domain.Enums;

namespace TaskSoliq.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }

        [Required]
        public EmployeeCategory EmployeeCategory { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public Status Status { get; set; }


    }
}
