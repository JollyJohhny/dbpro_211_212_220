using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class ClassViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CourseId { get; set; }
    }
}