using Microsoft.EntityFrameworkCore;
 
namespace User_Dashboard.Models
{
    public class UserDashboardContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public UserDashboardContext(DbContextOptions<UserDashboardContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}