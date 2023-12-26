using BluegrassDigitalPeopleDirectory.Models.TmpProc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static BluegrassDigitalPeopleDirectory.Models.Pub;

namespace BluegrassDigitalPeopleDirectory.Models
{
    public class DBContext(DbContextOptions<DBContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<ErrorLog> Errors { get; set; }
        public DbSet<TmpTask> TmpTasks { get; set; }
        public DbSet<Person> People { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the unique index
            modelBuilder.Entity<TmpTask>()
                .HasIndex(u => u.WhenRun)
                .IsUnique();
        }
    }
}
