using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Management_System.Models;
using System.Collections;

namespace School_Management_System.Controllers
{
    public class AttendanceController : Controller
    {

        public ActionResult StudentAttendance()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities sc = new schooldbEntities();
            List<@class> list = (from classes in sc.classes
                                 select classes).ToList<@class>();
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (@class c in list)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = c.className;
                sli.Value = c.className;
                selectList.Add(sli);
            }
            selectList.ElementAt<@SelectListItem>(0).Selected = true;
            ViewBag.SLIList = selectList;
            return View();
        }

        public PartialViewResult SectionDropDown(String className)
        {
                
                schooldbEntities sc = new schooldbEntities();
                @class c = (from classes in sc.classes
                            select classes).Where<@class>(cl => cl.className.Equals(className))
                            .FirstOrDefault<@class>();
                List<section> sectionList = (from sections in sc.sections
                                             select sections).Where(s => s.classId == c.classId)
                                                 .ToList<section>();
                List<SelectListItem> sectionSelectList = new List<SelectListItem>();
                foreach (section sect in sectionList)
                {
                    SelectListItem sli = new SelectListItem();
                    sli.Text = sect.sectionName;
                    sli.Value = sect.sectionName;
                    sectionSelectList.Add(sli);
                }
                if(sectionSelectList.Count>0)               
                    sectionSelectList.ElementAt<SelectListItem>(0).Selected = true;
                ViewBag.SLIList = sectionSelectList;
                return PartialView();
               
        }


        public PartialViewResult FetchStudentsForAttendance(string className, string sectionName)
        {
            schooldbEntities sc = new schooldbEntities();
            int classId = (from classes in sc.classes
                           select classes).Where<@class>(c=>c.className.Equals(className)).FirstOrDefault<@class>().classId;
            int sectionId=0;
            try
            {
                sectionId = (from sections in sc.sections
                                 select sections).Where<section>(s => s.classId == classId && s.sectionName.Equals(sectionName))
                                 .FirstOrDefault<section>().sectionId;
            }
            catch (Exception ex)
            { 
            
            }
            List<student> studentList = (from students in sc.students
                                         select students).Where<student>(s=>s.sectionId == sectionId).ToList<student>();
            ViewBag.studentList = studentList;
            return PartialView();
        }


        public void SaveAttendance(AttendanceInfo ai)
        {
            schooldbEntities sc = new schooldbEntities();
            attendance at = (from att in sc.attendances
                             select att).Where<attendance>(entry => entry.teacherId == ai.teacherId
                             && entry.AttendanceDate.Year == ai.enterYear
                             && entry.AttendanceDate.Month == ai.enterMonth
                             && entry.AttendanceDate.Day == ai.enterDay).FirstOrDefault();
            at.IsPresent = ai.isPresent;
            DateTime dt1 = new DateTime(ai.enterYear, ai.enterMonth,
                ai.enterDay, ai.enterHour, ai.enterMinute, 0);
            at.AttendanceDate = dt1;
            DateTime dt2 = new DateTime(ai.enterYear, ai.enterMonth,
                ai.enterDay, ai.exitHour, ai.exitMinute, 0);
            at.AttendanceExitDate = dt2;
            sc.SaveChanges();
        }

        public void MarkStudentAttendance(StudentPresentId[] ids)
        {
            schooldbEntities sc = new schooldbEntities();
            foreach (StudentPresentId spi in ids)
            {
                studentattendance sat = new studentattendance();
                sat.StudentId = spi.id;
                sat.Date = DateTime.Now;
                sc.studentattendances.AddObject(sat);
            }
            try
            {
                sc.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public ActionResult EditDailyAttendance(AttendanceInfo ai)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View(ai);
        }

        public ActionResult Timing()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult Holidays()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult Leave()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult MarkHoliday(string dateString)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int Day = Convert.ToInt32(dateString.Split('/')[1]);
            int Month = Convert.ToInt32(dateString.Split('/')[0]);
            int Year = Convert.ToInt32(dateString.Split('/')[2]);
            schooldbEntities sc = new schooldbEntities();
            List<teacher> allTeachers = (from t in sc.teachers
                                         select t).ToList<teacher>();
            foreach (teacher teacherObject in allTeachers)
            {
                attendance att = new attendance();
                att.teacherId = teacherObject.teacherId;
                DateTime dt = new DateTime(Year, Month, Day);
                att.AttendanceDate = dt;
                att.IsPresent = "Holiday";
                sc.attendances.AddObject(att);
            }
            sc.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult MarkLeave(string leaves)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            string[] leavesStrings = leaves.Split(',');
            schooldbEntities sc = new schooldbEntities();
            List<attendance> attendances = (from attend in sc.attendances
                                            select attend).ToList<attendance>();
            foreach (string leave in leavesStrings)
            {
                string[] credentials = leave.Split(':');
                int id = Convert.ToInt32(credentials[0].Split('\"')[1]);
                int fromDay = Convert.ToInt32(credentials[1].Split('/')[1]);
                int fromMonth = Convert.ToInt32(credentials[1].Split('/')[0]);
                int fromYear = Convert.ToInt32(credentials[1].Split('/')[2]);
                int toDay = Convert.ToInt32(credentials[2].Split('/')[1]);
                int toMonth = Convert.ToInt32(credentials[2].Split('/')[0]);
                string temp = credentials[2].Split('/')[2].Split('\\')[0];
                int toYear = Convert.ToInt32(credentials[2].Split('/')[2].Split('\"')[0]);
                DateTime fromDate = new DateTime(fromYear, fromMonth, fromDay);
                DateTime toDate = new DateTime(toYear, toMonth, toDay);
                for (int i = 0; fromDate.AddDays(i).Date != toDate.Date; i++)
                {
                    attendance attendanceObj = new attendance();
                    attendanceObj.AttendanceDate = fromDate.AddDays(i);
                    attendanceObj.teacherId = id;
                    attendanceObj.IsPresent = "Leave";
                    sc.attendances.AddObject(attendanceObj);
                }
            }
            sc.SaveChanges();
            return View();
        }

        public ActionResult UpdateTime(int fridaystarthour, int fridaystartmin,
                            int fridayendhour, int fridayendmin,
                            int normalstarthour, int normalstartmin,
                            int normalendhour, int normalendmin)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities sc = new schooldbEntities();
            timing friday = (from time in sc.timings
                             select time).Where<timing>(t => t.TimingsId == 2).FirstOrDefault<timing>();
            timing normal = (from time in sc.timings
                             select time).Where<timing>(t => t.TimingsId == 1).FirstOrDefault<timing>();
            TimeSpan? ts = new TimeSpan(fridaystarthour, fridaystartmin, 0);
            friday.TimeIn = ts;
            ts = new TimeSpan(fridayendhour, fridayendmin, 0);
            friday.TimeOut = ts;
            ts = new TimeSpan(normalstarthour, normalstartmin, 0);
            normal.TimeIn = ts;
            ts = new TimeSpan(normalendhour, normalendmin, 0);
            normal.TimeOut = ts;
            sc.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index(string designation = "")
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            if (designation.Equals(""))
            {
                designation = (from teachers in new schooldbEntities().teachers select teachers).FirstOrDefault<teacher>().Designation;
            }


            List<teacher> teachersList = new List<teacher>();
            teachersList = (from teachers in new schooldbEntities().teachers
                            select teachers).Where<teacher>(t => t.Designation.Equals(designation)).ToList<teacher>();
            return View(teachersList);

        }

        public PartialViewResult AttendanceHead()
        {
            return PartialView();
        }

        public ViewResult EditAttendance()
        {
            ViewBag.startYear = 2013;
            ViewBag.endYear = DateTime.Now.ToUniversalTime().AddHours(5).Year;
            ViewBag.startMonth = 3;
            ViewBag.endMonth = DateTime.Now.ToUniversalTime().AddHours(5).Month + 1;
            return View();
        }

        public PartialViewResult EmployeeTypeHead()
        {
            IList<teacher> teachersWithUniqueDesignations = (from t in new schooldbEntities().teachers
                                                             select t).ToList<teacher>();
            teachersWithUniqueDesignations = (from t in teachersWithUniqueDesignations
                                              select t).Distinct<teacher>
                                              (new DesignationEqualityComparer())
                                              .ToList<teacher>();
            ViewBag.uniqueDesignation = teachersWithUniqueDesignations;
            return PartialView();
        }

        public PartialViewResult ReturnAttendance(int year, int month, string designation)
        {
            schooldbEntities entities = new schooldbEntities();

            ViewBag.year = year;
            ViewBag.month = month;

            ViewBag.teacherList = (from teacher t in entities.teachers
                                   select t).Where<teacher>(ta => ta.Designation.Equals(designation))
                                   .ToList<teacher>();
            ViewBag.attendanceList = (from attendance a in entities.attendances
                                      select a).Where(att => att.AttendanceDate.Year == year
                                          && att.AttendanceDate.Month == month).ToList<attendance>();
            ViewBag.daysList = new List<int>();
            for (int i = 1; i < 32; i++)
            {
                try
                {
                    int ret = 0;
                    DateTime dateTime = new DateTime(year, month, i);
                    if ((ret = DateTime.Compare(dateTime.Date, DateTime.Now.ToUniversalTime().AddHours(5).Date)) < 0 && dateTime.DayOfWeek != DayOfWeek.Sunday)
                        ViewBag.daysList.Add(i);
                }
                catch (ArgumentOutOfRangeException ex)
                {

                }

            }
            return PartialView();
        }



        public PartialViewResult ReturnAttendanceToBeChanged(string date, string designation)
        {
            int day = Convert.ToInt32(date.Split('/')[1]);
            int month = Convert.ToInt32(date.Split('/')[0]);
            int year = Convert.ToInt32(date.Split('/')[2]);
            schooldbEntities sc = new schooldbEntities();
            List<teacher> teacherList = (from teacher t in sc.teachers
                                         select t).Where<teacher>(ta => ta.Designation.Equals(designation))
                                   .ToList<teacher>();
            List<attendance> attendanceList = (from attendance a in sc.attendances
                                               select a).Where(att => att.AttendanceDate.Year == year
                                          && att.AttendanceDate.Month == month &&
                                          att.AttendanceDate.Day == day).ToList<attendance>();
            var attendanceInfoList = from t in teacherList
                                     join a in attendanceList
                                         on t.teacherId equals a.teacherId
                                     select new
                                     {
                                         t.teacherId,
                                         t.TeacherName,
                                         a.IsPresent,
                                         a.AttendanceDate,
                                         a.AttendanceExitDate
                                     };
            List<AttendanceInfo> attendanceInfoListActual = new List<AttendanceInfo>();
            foreach (var value in attendanceInfoList)
            {
                AttendanceInfo ai = new AttendanceInfo();
                ai.teacherId = value.teacherId;
                ai.teacherName = value.TeacherName;
                ai.enterDay = value.AttendanceDate.Day;
                ai.enterMonth = value.AttendanceDate.Month;
                ai.enterYear = value.AttendanceDate.Year;
                ai.enterMinute = value.AttendanceDate.Minute;
                ai.enterHour = value.AttendanceDate.Hour;
                if (value.AttendanceExitDate != null)
                {
                    ai.exitDay = value.AttendanceExitDate.Value.Day;
                    ai.exitMonth = value.AttendanceExitDate.Value.Month;
                    ai.exitYear = value.AttendanceExitDate.Value.Year;
                    ai.exitMinute = value.AttendanceExitDate.Value.Minute;
                    ai.exitHour = value.AttendanceExitDate.Value.Hour;
                }
                ai.isPresent = value.IsPresent;
                attendanceInfoListActual.Add(ai);
            }
            return PartialView(attendanceInfoListActual);
        }



        public ActionResult ViewAbsents()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.startYear = 2013;
            ViewBag.endYear = DateTime.Now.ToUniversalTime().AddHours(5).Year;
            ViewBag.startMonth = 3;
            ViewBag.endMonth = DateTime.Now.ToUniversalTime().AddHours(5).Month + 1;
            return View();
        }


        public PartialViewResult ReturnAbsents(int year, int month)
        {
            schooldbEntities entities = new schooldbEntities();

            ViewBag.year = year;
            ViewBag.month = month;

            ViewBag.teacherList = (from teacher t in entities.teachers
                                   select t).ToList<teacher>();
            ViewBag.attendanceList = (from attendance a in entities.attendances
                                      select a).Where(att => att.AttendanceDate.Year == year
                                          && att.AttendanceDate.Month == month).ToList<attendance>();
            ViewBag.daysList = new List<int>();
            for (int i = 1; i < 32; i++)
            {
                try
                {
                    int ret = 0;
                    DateTime dateTime = new DateTime(year, month, i);
                    if ((ret = DateTime.Compare(dateTime.Date, DateTime.Now.ToUniversalTime().AddHours(5).Date)) < 0 && dateTime.DayOfWeek != DayOfWeek.Sunday)
                        ViewBag.daysList.Add(i);
                }
                catch (ArgumentOutOfRangeException ex)
                {

                }

            }
            return PartialView();
        }








        public PartialViewResult ReturnReport(int year, int month, int teacherId)
        {
            schooldbEntities entities = new schooldbEntities();

            ViewBag.year = year;
            ViewBag.month = month;

            ViewBag.teacher = (from teacher t in entities.teachers
                               select t).ToList<teacher>().Where<teacher>(te => te.teacherId == teacherId).FirstOrDefault<teacher>();
            ViewBag.attendanceList = (from attendance a in entities.attendances
                                      select a).Where(att => att.AttendanceDate.Year == year
                                          && att.AttendanceDate.Month == month && att.teacherId == teacherId).ToList<attendance>();
            return PartialView();
        }




        public ActionResult ViewAttendance()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.startYear = 2013;
            ViewBag.endYear = DateTime.Now.ToUniversalTime().AddHours(5).Year;
            ViewBag.startMonth = 3;
            ViewBag.endMonth = DateTime.Now.ToUniversalTime().AddHours(5).Month + 1;
            return View();
        }

        public PartialViewResult ReturnReportsList(int year, int month, string designation)
        {
            schooldbEntities entities = new schooldbEntities();

            ViewBag.year = year;
            ViewBag.month = month;

            ViewBag.teacherList = (from teacher t in entities.teachers
                                   select t).Where<teacher>(ta => ta.Designation.Equals(designation))
                                   .ToList<teacher>();
            ViewBag.attendanceList = (from attendance a in entities.attendances
                                      select a).Where(att => att.AttendanceDate.Year == year
                                          && att.AttendanceDate.Month == month).ToList<attendance>();
            ViewBag.daysList = new List<int>();
            for (int i = 1; i < 32; i++)
            {
                try
                {
                    int ret = 0;
                    DateTime dateTime = new DateTime(year, month, i);
                    if ((ret = DateTime.Compare(dateTime.Date, DateTime.Now.ToUniversalTime().AddHours(5).Date)) < 0 && dateTime.DayOfWeek != DayOfWeek.Sunday)
                        ViewBag.daysList.Add(i);
                }
                catch (ArgumentOutOfRangeException ex)
                {

                }

            }
            return PartialView();
        }

        public ActionResult ViewSalaryReports()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.startYear = 2013;
            ViewBag.endYear = DateTime.Now.ToUniversalTime().AddHours(5).Year;
            ViewBag.startMonth = 3;
            ViewBag.endMonth = DateTime.Now.ToUniversalTime().AddHours(5).Month + 1;
            return View();
        }

        [HttpPost]
        public void MarkAttendance(JSONAttendance[] att)
        {
            timing fridayTiming = (from t in new schooldbEntities().timings
                                   select t).Where<timing>(time => time.TimingsId == 2).FirstOrDefault<timing>();
            timing otherTiming = (from t in new schooldbEntities().timings
                                  select t).Where<timing>(time => time.TimingsId == 1).FirstOrDefault<timing>();

            schooldbEntities sc = new schooldbEntities();
            foreach (JSONAttendance at in att)
            {
                if (at.In)
                {
                    attendance a = new attendance();
                    a.teacherId = at.TeacherId;
                    DateTime today = DateTime.Now.ToUniversalTime().AddHours(5);
                    DateTime dt = new DateTime(today.Year, today.Month, today.Day, at.Hour, at.Min, 0);
                    a.AttendanceDate = dt;



                    if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                    {

                        try
                        {
                            double diff = a.AttendanceDate.TimeOfDay.TotalMinutes - fridayTiming.TimeIn.Value.TotalMinutes;
                            if (diff > 0 && diff < 15)
                            {
                                a.IsPresent = "Late";
                            }
                            if (diff >= 15 && diff < 30)
                            {
                                a.IsPresent = "Double Late";
                            }
                            if (diff >= 30)
                            {
                                a.IsPresent = "Half Day";
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    else
                    {

                        try
                        {
                            double diff = a.AttendanceDate.TimeOfDay.TotalMinutes - otherTiming.TimeIn.Value.TotalMinutes;
                            if (diff > 0 && diff < 15)
                            {
                                a.IsPresent = "Late";
                            }
                            if (diff >= 15 && diff < 30)
                            {
                                a.IsPresent = "Double Late";
                            }
                            if (diff >= 30)
                            {
                                a.IsPresent = "Half Day";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    sc = new schooldbEntities();
                    sc.attendances.AddObject(a);
                    sc.SaveChanges();
                }
                else
                {

                    attendance a = (from ate in sc.attendances
                                    select ate).ToList<attendance>().Where<attendance>(attend =>
                                    attend.teacherId == at.TeacherId &&
                                    attend.AttendanceDate.Date == DateTime.Now.ToUniversalTime().AddHours(5).Date
                                    ).FirstOrDefault<attendance>();
                    DateTime today = DateTime.Now.ToUniversalTime().AddHours(5);
                    DateTime dt = new DateTime(today.Year, today.Month, today.Day, at.Hour, at.Min, 0);
                    a.AttendanceExitDate = dt;
                    sc.SaveChanges();
                }

            }
        }

        public class DesignationEqualityComparer : IEqualityComparer<teacher>
        {
            public bool Equals(teacher t1, teacher t2)
            {
                if (t1.Designation.Equals(t2.Designation))
                    return true;
                return false;
            }

            public int GetHashCode(teacher t)
            {
                return t.Designation.GetHashCode();
            }
        }
    }
}
