using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class NewsViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}