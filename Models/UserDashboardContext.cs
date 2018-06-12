using Microsoft.EntityFrameworkCore;
 
namespace User_Dashboard.Models
{
    public class UserDashboardContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.ProfileMessaged)
                .WithMany(u => u.ProfileMessages)
                .HasForeignKey(m => m.ProfileMessagedUserId);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.MessagePoster)
                .WithMany(u => u.MessagesPosted)
                .HasForeignKey(m => m.MessagePosterUserId);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.CommentedMessage)
                .WithMany(m => m.MessageComments)
                .HasForeignKey(c => c.MessageId);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.CommentPoster)
                .WithMany(u => u.CommentsPosted)
                .HasForeignKey(c => c.UserId);
        }
        public UserDashboardContext(DbContextOptions<UserDashboardContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}