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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Identity Customizd entities
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.ToTable(name: "User");
            //});
        }
        public struct ConnectionString
        {
            public string Dev { get; set; }
            public string Test { get; set; }
            public string Prod { get; set; }
        }
    }
}
