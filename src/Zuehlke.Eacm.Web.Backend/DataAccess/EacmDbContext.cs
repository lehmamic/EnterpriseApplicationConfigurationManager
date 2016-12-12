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

        public DbSet<ConfigurationProject> Projects => this.Set<ConfigurationProject>();

        public DbSet<ConfigurationEntity> Entities => this.Set<ConfigurationEntity>();

        public DbSet<ConfigurationProperty> Properties => this.Set<ConfigurationProperty>();

        public DbSet<ConfigurationEntry> Entries => this.Set<ConfigurationEntry>();

        public DbSet<ConfigurationValue> Values => this.Set<ConfigurationValue>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationProject>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<ConfigurationEntity>()
                .HasIndex(p => new { p.ProjectId, p.Name })
                .IsUnique();

            modelBuilder.Entity<ConfigurationProperty>()
                .HasIndex(p => new { p.EntityId, p.Name })
                .IsUnique();

            modelBuilder.Entity<ConfigurationValue>()
                .HasIndex(p => new { p.EntryId, p.PropertyId })
                .IsUnique();
        }
    }
}