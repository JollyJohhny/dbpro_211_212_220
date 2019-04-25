using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string RegisterationNumber { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}