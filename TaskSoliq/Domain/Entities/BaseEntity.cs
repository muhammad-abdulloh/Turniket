namespace TaskSoliq.Domain.Entities
{
    /// <summary>
    /// most common base entity
    /// </summary>
    public class BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// status dates
        /// </summary>
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
