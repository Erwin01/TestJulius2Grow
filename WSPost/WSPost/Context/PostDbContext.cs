using Microsoft.EntityFrameworkCore;
using WSPost.Models;

namespace WSPost.Context
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Login> Logins { get; set; }
    }
}
