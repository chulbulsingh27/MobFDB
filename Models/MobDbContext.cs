using Microsoft.EntityFrameworkCore;

namespace MobFDB.Models
{
    public class MobDbContext : DbContext
    {
        public MobDbContext(DbContextOptions<MobDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<User> Users
        { get; set; }
        public virtual DbSet<Product> Products{ get; set; }
        public virtual DbSet<Order> Orders { get; set; }

    }
}
