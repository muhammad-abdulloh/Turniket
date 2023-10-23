using Microsoft.EntityFrameworkCore;
using TaskSoliq.Domain.Entities;

namespace TaskSoliq.Infrastructure
{
    public class TurniketDbContext : DbContext
    {
        /// <summary>
        /// Connect Data Base
        /// </summary>
        /// <param name="options"></param>
        public TurniketDbContext(DbContextOptions<TurniketDbContext> options) : base(options) { }

        /// <summary>
        /// Create Users table in database
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
