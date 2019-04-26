using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class HostelViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Hostel Name")]
        public string HostelName { get; set; }

        [Display(Name = "Hostel Location")]
        public int HostelLocation { get; set; }
    }
}