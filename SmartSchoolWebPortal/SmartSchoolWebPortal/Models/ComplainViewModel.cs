using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class ComplainViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        [Display(Name = "Parent Id")]
        public int ParentId { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }

        [Display(Name = "Complaint Status")]
        public string Status { get; set; }

        public DateTime Date { get; set; }
    }
}