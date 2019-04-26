using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class HostelViewModel
    {

        [Display(Name = "Add Image")]
        public HttpPostedFileBase Image { get; set; }
        [Display(Name = "Hostel Image")]
        public string ImagePath { get; set; }


        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Hostel Name")]
        public string HostelName { get; set; }

        [Display(Name = "Hostel Location")]
        public string HostelLocation { get; set; }

        [Display(Name = "Hostel Details")]
        public string HostelDetails { get; set; }

        [Display(Name = "Hostel Rent")]
        public int HostelRent { get; set; }
    }
}