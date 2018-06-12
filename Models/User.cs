using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public class Register : BaseEntity
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
    public class Login : BaseEntity
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
    public class ChangePassword : BaseEntity
    {
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
    public class EditInfo : BaseEntity
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}