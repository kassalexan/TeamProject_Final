﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Models
{
    public class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string Mobile { get; set; }
        //public List<Problem> Problems { get; set; }
        public string District { get; set; }
        public bool Deleted { get; set; }
    }
}
