using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamProject.Models
{
    public class Location
    {
            public int Id { get; set; }
            public string Address { get; set; }
            public double Latitude { get; set; }
            public double Longtitude { get; set; }
            public string State { get; set; }
    }
}