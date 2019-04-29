using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class ClassViewModel
    {
        [Display(Name = "Class Id")]
        public int Id { get; set; }

        [Display(Name = "Class Title")]
        public string Title { get; set; }

        [Display(Name = "Class Course")]
        public int CourseId { get; set; }
    }
}