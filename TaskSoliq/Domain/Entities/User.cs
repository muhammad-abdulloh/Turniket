using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TaskSoliq.Domain.Entities
{
    public class User : BaseEntity
    {
        [IncludeInReport(Order = 2)]
        public string? FirstName { get; set; }

        [IncludeInReport(Order = 3)]
        public string? LastName { get; set; }

        [IncludeInReport(Order = 4)]
        public int? Age { get; set; }

        [Required]
        [IncludeInReport(Order = 5)]
        public int EmployeeCategory { get; set; }

        [IncludeInReport(Order = 6)]
        public string? ImageUrl { get; set; }

        [Required]
        [IncludeInReport(Order = 7)]
        public int Status { get; set; }


    }
}
