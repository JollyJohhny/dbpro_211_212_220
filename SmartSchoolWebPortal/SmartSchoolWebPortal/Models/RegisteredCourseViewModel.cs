using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class RegisteredCourseViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Course Id")]
        public int CourseId { get; set; }

        [Display(Name = "Student Id")]
        public string StudentId { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Course Name")]
        public string Name { get; set; }
    }
}