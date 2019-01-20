using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamProject.Models
{
    public class UserVM2
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Role role { get; set; }
        public string Mobile { get; set; }
        public string District { get; set; }
        public bool Deleted { get; set; }
    }
}