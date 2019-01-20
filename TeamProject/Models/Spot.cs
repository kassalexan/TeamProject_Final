using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Models
{
    public class Spot
    {
        public int SpotId { get; set; }
        public int SpotCategoryId { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public int PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string District { get; set; }
        public string State { get; set; }
    }
}
