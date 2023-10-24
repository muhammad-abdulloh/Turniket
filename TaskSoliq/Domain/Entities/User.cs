using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSoliq.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }

        [Required]
        public int EmployeeCategory { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public int Status { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

    }
}
