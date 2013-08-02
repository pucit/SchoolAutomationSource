using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Management_System.Models
{
    public class AttendanceInfo
    {
        public int teacherId { get; set; }
        public string teacherName { get; set; }
        public int enterDay { get; set; }
        public int enterMonth { get; set; }
        public int enterYear { get; set; }
        public int enterHour { get; set; }
        public int enterMinute { get; set; }
        public int exitDay { get; set; }
        public int exitMonth { get; set; }
        public int exitYear { get; set; }
        public int exitHour { get; set; }
        public int exitMinute { get; set; }
        public string isPresent { get; set; }
    }
}