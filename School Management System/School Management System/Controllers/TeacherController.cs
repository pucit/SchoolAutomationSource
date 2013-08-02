using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        schooldbEntities DAO = new schooldbEntities();
        public static String desig;
        public static String egender;
        public static String epayment;
        public static String emarital;
        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            teacher tec = (from tech in DAO.teachers where tech.teacherId == id select tech).FirstOrDefault<teacher>();
            tec.Status = "0";
            DAO.SaveChanges();

            return RedirectToAction("ViewTeacherMenu");
        }
        
        public ActionResult Edit(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> maritalList = new List<String>();
            maritalList.Add("Married");
            maritalList.Add("Un-Married");

            List<String> paymentList = new List<String>();
            paymentList.Add("Cash");
            paymentList.Add("Bank");

            List<String> statusList = new List<String>();
            statusList.Add("1");
            statusList.Add("0");

            List<String> genderList = new List<String>();
            genderList.Add("Male");
            genderList.Add("Female");

            List<String> designationList = new List<String>();
            designationList.Add("Senior Teacher");
            designationList.Add("Junior Teacher");
            designationList.Add("Administration");
            designationList.Add("Security");
            designationList.Add("Cleanliness");
            designationList.Add("Accounts");

            ViewBag.maritalList = maritalList;
            ViewBag.genderList = genderList;
            ViewBag.designationList = designationList;
            ViewBag.statusList = statusList;
            ViewBag.paymentList = paymentList;
            return View((from T in DAO.teachers where T.teacherId == id select T).FirstOrDefault<teacher>());
        }

        [HttpPost]
        public ActionResult Edit(int id, teacher T)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            teacher tec = (from tech in DAO.teachers where tech.teacherId == id select tech).FirstOrDefault<teacher>();
            tec.teacherId = T.teacherId;
            tec.Status = T.Status;
            tec.TeacherAddress = T.TeacherAddress;
            tec.TeacherDOB = T.TeacherDOB;
            tec.TeacherName = T.TeacherName;
            tec.Telephone = T.Telephone;
            tec.CellPhone = T.CellPhone;
            tec.JoinDate = T.JoinDate;
            tec.BasicSalary = T.BasicSalary;
            tec.Account_Number = T.Account_Number;
            tec.MaritalStatus = T.MaritalStatus;
            tec.PaymentMode = T.PaymentMode;
            tec.Designation = T.Designation;
            
            DAO.SaveChanges();

            return RedirectToAction("ViewTeacherMenu");
        }


        public ActionResult AddTeacher()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> maritalList = new List<String>();
            maritalList.Add("Married");
            maritalList.Add("Un-Married");

            List<String> paymentList = new List<String>();
            paymentList.Add("Cash");
            paymentList.Add("Bank");

            List<String> statusList = new List<String>();
            statusList.Add("1");
            statusList.Add("0");

            List<String> genderList = new List<String>();
            genderList.Add("Male");
            genderList.Add("Female");

            List<String> designationList = new List<String>();
            designationList.Add("Senior Teacher");
            designationList.Add("Junior Teacher");
            designationList.Add("Administration");
            designationList.Add("Security");
            designationList.Add("Cleanliness");
            designationList.Add("Accounts");

            ViewBag.maritalList = maritalList;
            ViewBag.genderList = genderList;
            ViewBag.designationList = designationList;
            ViewBag.statusList = statusList;
            ViewBag.paymentList = paymentList;

            return View();
        }

        [HttpPost]
        public ActionResult AddTeacher(teacher T)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            DAO.teachers.AddObject(T);
            DAO.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ViewTeacherMenu()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult ActiveView()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from emp in DAO.teachers where emp.Status.Equals("1") select emp).ToList<teacher>());
        }

        public ActionResult InActiveView()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from emp in DAO.teachers where emp.Status.Equals("0") select emp).ToList<teacher>());
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult DesignationView()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> designationList = new List<String>();
            designationList.Add("Senior Teacher");
            designationList.Add("Junior Teacher");
            designationList.Add("Administration");
            designationList.Add("Security");
            designationList.Add("Cleanliness");
            designationList.Add("Accounts");

            ViewBag.designationList = designationList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult DesignationView(int id, teacher std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                desig = std.Designation.ToString();
                return RedirectToAction("DesignationViewResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("DesignationView");
            }
        }

        public ActionResult DesignationViewResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.teachers where ((list.Designation.Equals(desig)) && (list.Status.Equals("1"))) select list).ToList<teacher>());
            }
            catch (Exception E)
            {
                return RedirectToAction("DesignationView/1");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult GenderView(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> genderList = new List<String>();
            genderList.Add("Male");
            genderList.Add("Female");

            ViewBag.genderList = genderList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult GenderView(int id, teacher std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                egender = std.Gender.ToString();
                return RedirectToAction("GenderViewResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("GenderView");
            }
        }

        public ActionResult GenderViewResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.teachers where ((list.Gender == egender) && (list.Status.Equals("1"))) select list).ToList<teacher>());
            }
            catch (Exception E)
            {
                return View("GenderView");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult CashView(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> paymentList = new List<String>();
            paymentList.Add("Cash");
            paymentList.Add("Bank");

            ViewBag.paymentList = paymentList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult CashView(int id, teacher std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                epayment = std.PaymentMode.ToString();
                return RedirectToAction("CashViewResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("CashView");
            }
        }

        public ActionResult CashViewResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.teachers where ((list.PaymentMode == epayment) && (list.Status.Equals("1"))) select list).ToList<teacher>());
            }
            catch (Exception E)
            {
                return View("CashView");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult MaritalView(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> maritalList = new List<String>();
            maritalList.Add("Married");
            maritalList.Add("Un-Married");

            ViewBag.maritalList = maritalList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult MaritalView(int id, teacher std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                emarital = std.MaritalStatus.ToString();
                return RedirectToAction("MaritalViewResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("MaritalView");
            }
        }

        public ActionResult MaritalViewResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.teachers where ((list.MaritalStatus == emarital) && (list.Status.Equals("1"))) select list).ToList<teacher>());
            }
            catch (Exception E)
            {
                return View("MaritalView");
            }

        }

        
    }
}
