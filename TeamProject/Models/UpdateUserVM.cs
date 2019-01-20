using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamProject.Models
{
    public class UpdateUserVM
    {
        [Required(ErrorMessage = "A Username is required.")]
        [Display(Name = "User Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must have min length of 3 and max Length of 50.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Current Password is required.")]
        [Display(Name = "Current Password")]

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must have min length of 8 and max Length of 50.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8}$",ErrorMessage = "Password must meet requirements")]
        public string NewPassword { get; set; }


        [Display(Name = "Confirm Password")]
        [Compare(("NewPassword"), ErrorMessage = "The New Password and Confirm New Password fields did not match.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[StringLength(16, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have min length of 3 and max Length of 50.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have min length of 3 and max Length of 50.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "This field must be 10 characters.")]
        [RegularExpression("[0-9]+")]
        public string Mobile { get; set; }

        public string District { get; set; }

        public int RoleId { get; set; }
    }
}