using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Models
{
    public class Problem
    {
        [Key]
        public int ProbId { get; set; }
        public Spot Spot { get; set; }
        public int SpotId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public ProbCategory Category { get; set; }
        public string Note { get; set; }

    }
}
