using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required.")]
        [MinLength(1, ErrorMessage = "User ID must be at least one character.")]
        [MaxLength(20, ErrorMessage = "User ID must be at most 20 characters.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters.")]
        [MaxLength(100, ErrorMessage = "Password must be at most 100 characters.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your confirm password does not match your password.")]
        public string ConfirmPassword { get; set; }
    }
}