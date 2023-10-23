using Fingers10.ExcelExport.Attributes;

namespace TaskSoliq.Domain.Entities
{
    /// <summary>
    /// most common base entity
    /// </summary>
    public class BaseEntity
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// status dates
        /// </summary>
        [IncludeInReport(Order = 8)]
        public DateTimeOffset? CreatedDate { get; set; }

        [IncludeInReport(Order = 9)]
        public DateTimeOffset? ModifyDate { get; set; }

        [IncludeInReport(Order = 10)]
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
