using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestNewLine.Data.Models;

namespace TestNewLine.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PostBlog> PostBlogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}