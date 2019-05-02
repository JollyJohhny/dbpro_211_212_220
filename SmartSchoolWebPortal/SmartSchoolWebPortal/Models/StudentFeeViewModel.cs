using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class StudentFeeViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Id")]
        public string StudentId { get; set; }

        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}