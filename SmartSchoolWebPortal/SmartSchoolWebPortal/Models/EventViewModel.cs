using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchoolWebPortal.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        public string Title { get; set; }
    }
}