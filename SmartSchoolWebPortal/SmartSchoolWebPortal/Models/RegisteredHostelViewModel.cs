using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class RegisteredHostelViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        [Display(Name = "Hostel Id")]
        public int HostelId { get; set; }

        [Display(Name = "Registeration Date")]
        public DateTime RegisterationDate { get; set; }
    }
}