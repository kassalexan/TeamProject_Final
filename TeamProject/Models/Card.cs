using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TeamProject.Models
{
    public class Card
    {
        [Key]
        public string CardNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int SecurityCode { get; set; }
        public string NameOnCard { get; set; }
        public string Country { get; set; }
        public User User { get; set; } //navigation property
        public int UserId { get; set; }
    }
}
