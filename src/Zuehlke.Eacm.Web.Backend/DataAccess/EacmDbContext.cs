using Microsoft.EntityFrameworkCore;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class EacmDbContext : DbContext
    {
        public EacmDbContext(DbContextOptions<EacmDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events => this.Set<Event>();
    }
}