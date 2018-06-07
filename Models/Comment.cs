using System;
using System.ComponentModel.DataAnnotations;

namespace User_Dashboard.Models
{
    public class Comment : BaseEntity
    {
        [Key]
        public int CommentId { get; set; }
        public int MessageId { get; set; }
        [Required]
        [Display(Name = "Message Text")]
        public string CommentText { get; set; }
        public Message CommentedMessage { get; set; }
        public int UserID { get; set; }
        public User CommentPoster { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}