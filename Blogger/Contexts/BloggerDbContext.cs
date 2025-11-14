using Microsoft.EntityFrameworkCore;
using Blogger.Models;

namespace Blogger.Contexts
{
    public class BloggerDbContext : DbContext
    {
        public BloggerDbContext() { }

        public BloggerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Models.Blogger> Blog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=blog;user=root;password='';");
        }
    }
}
