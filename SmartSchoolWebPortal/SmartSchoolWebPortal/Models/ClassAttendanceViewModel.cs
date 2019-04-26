using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class ClassAttendanceViewModel
    {
        [Display(Name = "Class Attendance Id")]
        public int Id { get; set; }

        [Display(Name = "Class Id")]
        public int ClassId { get; set; }

        [Display(Name = "Section Id")]
        public int SectionId { get; set; }

        [Display(Name = "Attendace Date")]
        public DateTime Date { get; set; }
    }
}