using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Management_System.Models
{
    public class JSONAttendance
    {
        public int TeacherId { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public bool In { get; set; }
    }
}