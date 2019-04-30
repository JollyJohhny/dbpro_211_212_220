using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class HostelRequestViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Hostel Id")]
        public int HostelId { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        [Display(Name = "Request Status")]
        public int RequestStatus { get; set; }

        [Display(Name = "Hostel Name")]
        public string HostelName { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Display(Name = "Student Registeration Number")]
        public string StudentRegNo { get; set; }


    }
}