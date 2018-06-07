using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User_Dashboard.Models
{
    public class Message : BaseEntity
    {
        [Key]
        public int MessageId { get; set; }
        [Required]
        [Display(Name = "Message Text")]
        public string MessageText { get; set; }
        public int UserId { get; set; }
        public User MessagePoster { get; set; }
        public int ProfileId { get; set; }
        public User ProfileMessaged { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Comment> MessageComments { get; set; }
        public Message()
        {
            MessageComments = new List<Comment>();
        }
    }
}