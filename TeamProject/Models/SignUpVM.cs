using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeamProject.Models;

namespace TeamProject
{
    public class SignUpVM
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must have at least three(3) characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must have at least eight(8) characters")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8}$",ErrorMessage = "Password must meet requirements")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Display(Name = "Confirm Password")]
        [Compare(("Password"), ErrorMessage = "The Password and Confirm Password do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[StringLength(16, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Email must be valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have at least three(3) characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must have at least three(3) characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile  must have at least ten(10) digits")]
        [RegularExpression("[0-9]+")]
        public string Mobile { get; set; }

        public string District { get; set; }

        [Required(ErrorMessage = "Card required")]
        public CardVM Card { get; set; }

        //[Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }


    }
}