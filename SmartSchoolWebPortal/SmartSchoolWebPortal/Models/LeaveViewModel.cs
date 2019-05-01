using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class LeaveViewModel
    {
        [Display(Name = "Leave Id")]
        public int Id { get; set; }

        [Display(Name = "Leave Reason")]
        public string Reason { get; set; }

        [Display(Name = "Leave Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Leave Status")]
        public string Status { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }
    }
}