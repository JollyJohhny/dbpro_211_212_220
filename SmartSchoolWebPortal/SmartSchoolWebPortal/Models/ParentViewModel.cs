using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class ParentViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Contact")]
        public string Contact { get; set; }

        [Display(Name = "Registeration Number")]
        public string RegNo { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "CNIC number")]
        public string NIC { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Student Id")]
        public string StudentId { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }
    }
}