using Microsoft.EntityFrameworkCore;

namespace HighlyAvailableMonolithPOC.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
