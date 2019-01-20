using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Models
{
    public class CardVM
    {
        [Required(ErrorMessage = "Card Number is required")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Month is required")]
        [Range(1, 12, ErrorMessage = "Can only be between 1-12")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(2018, 2030, ErrorMessage = "Wrong Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Security code is required")]
        [Range(100, 999, ErrorMessage = "Can only have 3 characters")]
        public int SecurityCode { get; set; }

        [Required(ErrorMessage = "Name On Card is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "This field must have at least 5 characters")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "This field must have at least 3 characters")]
        public string Country { get; set; }
    }
}
