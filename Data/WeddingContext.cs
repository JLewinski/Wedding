using Microsoft.EntityFrameworkCore;

namespace Wedding.Data
{
    public class WeddingContext : DbContext
    {
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Note> Notes { get; set; }

    }
}