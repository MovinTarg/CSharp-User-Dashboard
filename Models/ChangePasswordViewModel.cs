using System;
using System.ComponentModel.DataAnnotations;

namespace User_Dashboard.Models
{
    public class ChangePasswordViewModel : BaseEntity
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
}