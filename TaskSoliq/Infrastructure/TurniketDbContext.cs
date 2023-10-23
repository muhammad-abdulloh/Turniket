using Microsoft.EntityFrameworkCore;
using TaskSoliq.Domain;

namespace TaskSoliq.Infrastructure
{
    public class TurniketDbContext : DbContext
    {
        public TurniketDbContext(DbContextOptions<TurniketDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null;
    }
}
