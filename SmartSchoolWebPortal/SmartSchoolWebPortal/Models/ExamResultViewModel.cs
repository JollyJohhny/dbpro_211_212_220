using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class ExamResultViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Exam Id")]
        public int ExamId { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        [Display(Name = "Obtained Marks")]
        public int ObtainedMarks { get; set; }

        [Display(Name = "Evaluation Date")]
        public DateTime EvaluationDate { get; set; }
    }
}