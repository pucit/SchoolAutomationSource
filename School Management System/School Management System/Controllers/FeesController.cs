using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class FeesController : Controller
    {
        //
        // GET: /Fees/

        public ActionResult Index(family id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            List<student> stdList = (from tmp in db.students where tmp.familyId == id.familyId && tmp.StudentStatus == 1 select tmp).ToList();
            List<double?> sum = (from tmp in db.students where tmp.familyId == id.familyId && tmp.StudentStatus == 1 select tmp.StudentFee).ToList();
            ViewBag.id = id.familyId;
            ViewBag.total = sum.Sum();
            return View(stdList);
        }

        public ActionResult YearDetails(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.id = id;
            return View(new MonthYearClassName());
        }

        //
        // GET: /Fees/Details/5

        public ActionResult Details(int id, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ReportStructure structure = new ReportStructure();
            ViewBag.year = year;
            List<student> stdList = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 select tmp).ToList();
            structure.stdList = stdList;
            List<double?> sum = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 select tmp.StudentFee).ToList();
            structure.feeList = sum;
            structure.totalFee = sum.Sum();
            List<fee> feeList = new List<fee>();
            foreach (student tmp in stdList)
            {
                List <fee> tmpFeeList = (from t in db.fees
                                            where tmp.studentId == t.studentId
                                            && t.Year == year
                                            select t).ToList();
                foreach (fee obj in tmpFeeList) feeList.Add(obj);
            }
            structure.feesList = feeList;
            double? discount = (from tmp in db.families where tmp.familyId == id select tmp.Discount).FirstOrDefault();
            structure.netFee = sum.Sum() - (discount / 100 * sum.Sum());
            return View(structure);
        }

        //
        // GET: /Fees/Create

        public ActionResult Create(int id, int total)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ViewBag.total = total;
            ViewBag.id = id;
            ViewBag.discount = (from tmp in db.families where tmp.familyId == id select tmp.Discount).FirstOrDefault();
            ViewBag.netFee = total - (ViewBag.discount / 100 * total);
            return View();
        } 

        //
        // POST: /Fees/Create

        [HttpPost]
        public ActionResult Create(int id, fee obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add insert logic here
                schooldbEntities db = new schooldbEntities();
                string day = obj.Is_Paid ? DateTime.Today.Day.ToString() : "";
                List<student> stdList = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 select tmp).ToList();
                List<double?> sum = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 select tmp.StudentFee).ToList();
                double? discount = (from tmp in db.families where tmp.familyId == id select tmp.Discount).FirstOrDefault();
                long feeId = (from tmp in db.fees select tmp).LongCount();
                List<fee> feeList = (from tmp in db.fees
                                      where tmp.Month == obj.Month && tmp.Year == obj.Year
                                      select tmp).ToList();
                foreach (fee tmp in feeList)
                {
                    foreach (student std in stdList)
                    {
                        if (tmp.studentId == std.studentId) return View("AddError");
                    }
                }
                double? netFee = sum.Sum() - (discount / 100 * sum.Sum());
                int i = 0;
                foreach (student tmp in stdList)
                {
                    fee newObj = new fee { Id = (int)feeId + 1, Basic_Fee = (float)tmp.StudentFee, Concession = 0
                    , Is_Paid = obj.Is_Paid, Month = obj.Month, Year = obj.Year, 
                    Net_Fee = i == 0 ? (float)(sum.Sum() - (discount / 100 * sum.Sum())) : 0, studentId = tmp.studentId
                   , Day = obj.Is_Paid ? DateTime.Today.Day : 0
                    };
                    db.fees.AddObject(newObj);
                    i++;
                }
                db.SaveChanges();
                return RedirectToAction("../Home/Family");
            }
            catch(Exception ex)
            {
                return View("AddError");
            }
        }
        
        //
        // GET: /Fees/Edit/5

        public ActionResult EditFee(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.id = id;
            return View();
        }

        public ActionResult Edit(int id, String month, String year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            List<double?> sum = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 
                                 select tmp.StudentFee).ToList();
            student std = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 select tmp).FirstOrDefault();
            var check = (from tmp in db.fees where std.studentId == tmp.studentId && tmp.Month == month && tmp.Year == year select tmp).FirstOrDefault();
            if(check == null)   return View("Error");
            ViewBag.sum = sum.Sum();
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.id = id;
            ViewBag.discount = (from tmp in db.families where tmp.familyId == id select tmp.Discount).FirstOrDefault();
            ViewBag.netFee = sum.Sum() - (ViewBag.discount / 100 * sum.Sum());
            return View();
        }

        //
        // POST: /Fees/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, fee obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add update logic here
                schooldbEntities db = new schooldbEntities();
                List<student> stdList = (from tmp in db.students where tmp.familyId == id && tmp.StudentStatus == 1 
                                             select tmp).ToList();
                foreach (student std in stdList)
                {
                    fee fee = (from tmp in db.fees where tmp.studentId == std.studentId && tmp.Month == obj.Month && tmp.Year == obj.Year select tmp).FirstOrDefault();
                    fee.Is_Paid = obj.Is_Paid;
                    if (obj.Is_Paid) fee.Day = DateTime.Today.Day;
                }
                db.SaveChanges();
                return RedirectToAction("../Home/Family");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Error()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        
        public ActionResult AddError()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult Reports()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult FeeDefaulter()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult FeePaid()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult DefaulterStudents()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult DefaulterStudentsList(string month, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ViewBag.month = month;
            ViewBag.year = year;
            return View((from tmp in db.fees where tmp.Month == month && tmp.Year == year
                             && tmp.Is_Paid == false select tmp).ToList());
        }

        public ActionResult DefaulterClassStudents()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult DefaulterClassStudentsList(string name, string month, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ViewBag.className = name;
            ViewBag.month = month;
            ViewBag.year = year;
            int classId = (from tmp in db.classes where tmp.className == name select tmp.classId).FirstOrDefault();
            List<student> stdList = (from tmp in db.students where tmp.classId == classId select tmp).ToList<student>();
            List<fee> feeList = new List<fee>();
            foreach(student tmp in stdList) feeList.Add((from t in db.fees where t.Month == month 
                                                             && t.Year == year && t.studentId == tmp.studentId
                                                             && t.Is_Paid == false select t).FirstOrDefault());
            return View(feeList);
        }

        public ActionResult PaidStudents()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult PaidStudentsList(string month, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ViewBag.month = month;
            ViewBag.year = year;
            var sum = (from tmp in db.fees
                         where tmp.Month == month && tmp.Year == year
                             && tmp.Is_Paid == true
                         select tmp.Net_Fee).ToList();
            ViewBag.sum = sum.Sum();
            return View((from tmp in db.fees
                         where tmp.Month == month && tmp.Year == year
                             && tmp.Is_Paid == true
                         select tmp).ToList());
        }

        public ActionResult PaidClassStudents()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult PaidClassStudentsList(string name, string month, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ViewBag.className = name;
            ViewBag.month = month;
            ViewBag.year = year;
            int classId = (from tmp in db.classes where tmp.className == name select tmp.classId).FirstOrDefault();
            List<student> stdList = (from tmp in db.students where tmp.classId == classId && tmp.StudentStatus == 1 select tmp).ToList<student>();
            List<double?> sum = new List<double?>();
            foreach (student tmp in stdList) sum.Add((from t in db.fees
                                                          where t.Month == month
                                                              && t.Year == year && t.studentId == tmp.studentId
                                                              && t.Is_Paid == true
                                                          select t.Net_Fee).FirstOrDefault());
            ViewBag.sum = sum.Sum();
            List<fee> feeList = new List<fee>();
            foreach (student tmp in stdList) feeList.Add((from t in db.fees
                                                          where t.Month == month
                                                              && t.Year == year && t.studentId == tmp.studentId
                                                              && t.Is_Paid == true
                                                          select t).FirstOrDefault());
            return View(feeList);
        }

        public ActionResult DailyFeeReport()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int day = DateTime.Today.Day;
            string m = DateTime.Today.Month.ToString(), year = DateTime.Today.Year.ToString(), month ="";
            ViewBag.day = day;
            ViewBag.year = year;
            switch (m)
            {
                case "1":
                    month = "January";
                    break;
                case "2":
                    month = "Februray";
                    break;
                case "3":
                    month = "March";
                    break;
                case "4":
                    month = "April";
                    break;
                case "5":
                    month = "May";
                    break;
                case "6":
                    month = "June";
                    break;
                case "7":
                    month = "July";
                    break;
                case "8":
                    month = "August";
                    break;
                case "9":
                    month = "September";
                    break;
                case "10":
                    month = "October";
                    break;
                case "11":
                    month = "November";
                    break;
                case "12":
                    month = "December";
                    break;
            }
            ViewBag.month = month;
            schooldbEntities db = new schooldbEntities();
            var sum = (from tmp in db.fees
                       where tmp.Day == day && tmp.Month == month
                           && tmp.Year == year && tmp.Is_Paid == true
                       select tmp.Net_Fee).ToList();
            ViewBag.sum = sum.Sum();
            return View((from tmp in db.fees where tmp.Day == day && tmp.Month == month 
                             && tmp.Year == year && tmp.Is_Paid == true select tmp).ToList());
        }

        public ActionResult SectionWiseFeeReport()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult SectionWiseFeeReportB(string name)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int cid = Convert.ToInt32(name);
            schooldbEntities db = new schooldbEntities();
            ViewBag.className = (from c in db.classes where c.classId == cid select c.className).First();
            return View();
        }

        public ActionResult SectionWiseFeeReportList(string name, string section, string month, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int cid = Convert.ToInt32(name);
            schooldbEntities db = new schooldbEntities();
            ViewBag.className = (from c in db.classes where c.classId == cid select c.className).First();
            ViewBag.classID = Convert.ToInt32(name);
            ViewBag.section = section;
            ViewBag.month = month;
            ViewBag.year = year;
            int classId = Convert.ToInt32(name);
            int sectionId = (from tmp in db.sections where tmp.sectionName.Equals(section) && tmp.classId == classId select tmp.sectionId).FirstOrDefault();

            String sectionN = (from tmp in db.sections where tmp.sectionId == sectionId && tmp.classId == classId select tmp.sectionName).FirstOrDefault();
            List<student> stdList = (from tmp in db.students where tmp.StudentStatus == 1
                                         && tmp.classId == classId && tmp.StudyClass.Equals(sectionN) select tmp).ToList();
            List<fee> feeList = new List<fee>();
            foreach (student std in stdList) feeList.Add((from tmp in db.fees where tmp.studentId == std.studentId
                                                           && tmp.Month == month && tmp.Year == year select tmp).FirstOrDefault());
            return View(feeList);
        }

        public ActionResult YearWiseFeeReport()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult YearWiseFeeReportList(string name, string year)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            schooldbEntities db = new schooldbEntities();
            ViewBag.className = name;
            ViewBag.year = year;
            int classId = (from tmp in db.classes where tmp.className == name select tmp.classId).First();
            ReportStructure structure = new ReportStructure();
            List<student> stdList = (from tmp in db.students where tmp.classId == classId && tmp.StudentStatus == 1
                                         select tmp).ToList();
            structure.stdList = stdList;
            List<double?> feeList1 = new List<double?>();
            List<double?> feeList2 = new List<double?>();
            List<double?> feeList3 = new List<double?>();
            List<double?> feeList4 = new List<double?>();
            List<double?> feeList5 = new List<double?>();
            List<double?> feeList6 = new List<double?>();
            List<double?> feeList7 = new List<double?>();
            List<double?> feeList8 = new List<double?>();
            List<double?> feeList9 = new List<double?>();
            List<double?> feeList10 = new List<double?>();
            List<double?> feeList11 = new List<double?>();
            List<double?> feeList12 = new List<double?>();
            Boolean isPaid = true;
            foreach (student std in stdList)
            {
                // 1
                 isPaid = (from tmp in db.fees where tmp.studentId == std.studentId
                                      && tmp.Year == year && tmp.Month == "January"
                                      select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList1.Add((double)std.StudentFee);
                else feeList1.Add(0);
                // 2
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "Februray"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList2.Add((double)std.StudentFee);
                else feeList2.Add(0);
                // 3
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "March"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList3.Add((double)std.StudentFee);
                else feeList3.Add(0);
                // 4
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "April"
                          select tmp.Is_Paid).FirstOrDefault();

                if (isPaid) feeList4.Add((double)std.StudentFee);
                else feeList4.Add(0);
                // 5
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "May"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList5.Add((double)std.StudentFee);
                else feeList5.Add(0);
                // 6
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "June"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList6.Add((double)std.StudentFee);
                else feeList6.Add(0);
                // 7
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "July"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList7.Add((double)std.StudentFee);
                else feeList7.Add(0);
                // 8
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "August"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList8.Add((double)std.StudentFee);
                else feeList8.Add(0);
                // 9
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "September"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList9.Add((double)std.StudentFee);
                else feeList9.Add(0);
                // 10
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "October"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList10.Add((double)std.StudentFee);
                else feeList10.Add(0);
                // 11
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "November"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList11.Add((double)std.StudentFee);
                else feeList11.Add(0);
                // 12
                isPaid = (from tmp in db.fees
                          where tmp.studentId == std.studentId
                              && tmp.Year == year && tmp.Month == "December"
                          select tmp.Is_Paid).FirstOrDefault();
                if (isPaid) feeList12.Add((double)std.StudentFee);
                else feeList12.Add(0);
            }

            structure.feeList1 = feeList1;
            structure.feeList2 = feeList2;
            structure.feeList3 = feeList3;
            structure.feeList4 = feeList4;
            structure.feeList5 = feeList5;
            structure.feeList6 = feeList6;
            structure.feeList7 = feeList7;
            structure.feeList8 = feeList8;
            structure.feeList9 = feeList9;
            structure.feeList10 = feeList10;
            structure.feeList11 = feeList11;
            structure.feeList12 = feeList12;

            List<fee> feesList1 = new List<fee>();
            List<fee> feesList2 = new List<fee>();
            List<fee> feesList3 = new List<fee>();
            List<fee> feesList4 = new List<fee>();
            List<fee> feesList5 = new List<fee>();
            List<fee> feesList6 = new List<fee>();
            List<fee> feesList7 = new List<fee>();
            List<fee> feesList8 = new List<fee>();
            List<fee> feesList9 = new List<fee>();
            List<fee> feesList10 = new List<fee>();
            List<fee> feesList11 = new List<fee>();
            List<fee> feesList12 = new List<fee>();

            foreach (student std in stdList)
            {
                // 1
                feesList1.Add((from tmp in db.fees where tmp.studentId == std.studentId 
                                  && tmp.Year == year && tmp.Month == "January"
                                  select tmp).FirstOrDefault());
                // 2
                feesList2.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "Februray"
                               select tmp).FirstOrDefault());
                // 3
                feesList3.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "March"
                               select tmp).FirstOrDefault());
                // 4
                feesList4.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "April"
                               select tmp).FirstOrDefault());
                // 5
                feesList5.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "May"
                               select tmp).FirstOrDefault());
                // 6
                feesList6.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "June"
                               select tmp).FirstOrDefault());
                // 7
                feesList7.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "July"
                               select tmp).FirstOrDefault());
                // 8
                feesList8.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "August"
                               select tmp).FirstOrDefault());
                // 9
                feesList9.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "September"
                               select tmp).FirstOrDefault());
                // 10
                feesList10.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "October"
                               select tmp).FirstOrDefault());
                // 11
                feesList11.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "November"
                               select tmp).FirstOrDefault());
                // 12
                feesList12.Add((from tmp in db.fees
                               where tmp.studentId == std.studentId
                                   && tmp.Year == year && tmp.Month == "December"
                               select tmp).FirstOrDefault());
            }
            structure.feesList1 = feesList1;
            structure.feesList2 = feesList2;
            structure.feesList3 = feesList3;
            structure.feesList4 = feesList4;
            structure.feesList5 = feesList5;
            structure.feesList6 = feesList6;
            structure.feesList7 = feesList7;
            structure.feesList8 = feesList8;
            structure.feesList9 = feesList9;
            structure.feesList10 = feesList10;
            structure.feesList11 = feesList11;
            structure.feesList12 = feesList12;
            return View(structure);
        }
    }
}

/*
 * @Html.DropDownListFor(model => model.Month, new List<SelectListItem> { 
           new SelectListItem{ Selected = false, Text = "January", Value = "January"},
           new SelectListItem{ Selected = false, Text = "Februray", Value = "Februray"},
           new SelectListItem{ Selected = false, Text = "March", Value = "March"},
           new SelectListItem{ Selected = false, Text = "April", Value = "April"},
           new SelectListItem{ Selected = false, Text = "May", Value = "May"},
           new SelectListItem{ Selected = false, Text = "June", Value = "June"},
           new SelectListItem{ Selected = false, Text = "July", Value = "July"},
           new SelectListItem{ Selected = false, Text = "August", Value = "August"},
           new SelectListItem{ Selected = false, Text = "September", Value = "September"},
           new SelectListItem{ Selected = false, Text = "October", Value = "October"},
           new SelectListItem{ Selected = false, Text = "November", Value = "November"},
           new SelectListItem{ Selected = false, Text = "December", Value = "December"},
           })
 * @Html.DropDownListFor(model => model.Year, new List<SelectListItem> { 
           new SelectListItem{ Selected = false, Text = "2013", Value = "2013"},
           new SelectListItem{ Selected = false, Text = "2014", Value = "2014"},
           new SelectListItem{ Selected = false, Text = "2015", Value = "2015"},
           new SelectListItem{ Selected = false, Text = "2016", Value = "2016"},
           new SelectListItem{ Selected = false, Text = "2017", Value = "2017"},
           new SelectListItem{ Selected = false, Text = "2018", Value = "2018"},
           new SelectListItem{ Selected = false, Text = "2019", Value = "2019"},
           new SelectListItem{ Selected = false, Text = "2020", Value = "2020"},
           new SelectListItem{ Selected = false, Text = "2021", Value = "2021"},
           new SelectListItem{ Selected = false, Text = "2022", Value = "2022"},
           new SelectListItem{ Selected = false, Text = "2023", Value = "2023"},
           })
*/