using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BluegrassDigitalPeopleDirectory.Models
{
    public class DBContext(DbContextOptions<DBContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Identity Customizd entities
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

        }
    }
}
