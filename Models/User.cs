using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User_Dashboard.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Message> MessagesPosted { get; set; }
        public List<Message> ProfileMessages { get; set; }
        public List<Comment> CommentsPosted { get; set; }
        public User()
        {
            MessagesPosted = new List<Message>();
            ProfileMessages = new List<Message>();
            CommentsPosted = new List<Comment>();
        }
    }
}