using Microsoft.EntityFrameworkCore;

namespace Blogger.Models
{
    public class BloggerDbContext : DbContext
    {
        public BloggerDbContext() { }

        public BloggerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Blogger> Blog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=blog;user=root;password='';");
        }
    }
}
