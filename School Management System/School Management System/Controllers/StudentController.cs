using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class StudentController : Controller
    {
        schooldbEntities DAO = new schooldbEntities();
        public static int cid;
        public static String csection;
        public static String cmedium;
        public static String cgender;
        public static int cstatus;


        //
        // GET: /Student/

        public ActionResult Index()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        //
        // GET: /Student/Details/5

        public ActionResult NewFamily()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult NewFamily(family fam)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            DAO.families.AddObject(fam);
            DAO.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ExistFamily()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from fam in DAO.families select fam).ToList());
        }

        //
        // GET: /Student/Create

        public ActionResult Create(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<@class> classList = new List<@class>();
            classList = DAO.classes.ToList<@class>();

            List<String> sectionList = new List<String>();
            sectionList.Add("BLUE");
            sectionList.Add("GREEN");
            sectionList.Add("RED");
            sectionList.Add("PINK");
            sectionList.Add("BROWN");
            sectionList.Add("BLACK");
            sectionList.Add("WHITE");
            sectionList.Add("ORANGE");
            sectionList.Add("PURPLE");

            List<String> genderList = new List<String>();
            genderList.Add("Male");
            genderList.Add("Female");

            List<String> nationalityList = new List<String>();
            nationalityList.Add("Pakistani");
            nationalityList.Add("Non-Pakistani");

            List<String> mediumList = new List<String>();
            mediumList.Add("English Medium");
            mediumList.Add("Urdu Medium");

            List<int> statusList = new List<int>();
            statusList.Add(1);
            statusList.Add(0);
            
            ViewBag.classList = classList;
            ViewBag.sectionList = sectionList;
            ViewBag.famId = id;
            ViewBag.famAddress = (from fam in DAO.families where fam.familyId==id select fam.FamilyAddress).First();
            ViewBag.pnum = (from fam in DAO.families where fam.familyId == id select fam.PhoneNumber).First();
            ViewBag.cnum = (from fam in DAO.families where fam.familyId == id select fam.CellNumber).First();
            ViewBag.genderList = genderList;
            ViewBag.nationalityList = nationalityList;
            ViewBag.mediumList = mediumList;
            ViewBag.statusList = statusList;
            
            return View(new student() { familyId=id});
        } 

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(int id, student s)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
                //try
                //{
                // TODO: Add insert logic here
            if (s.EducationMedium.Equals("English Medium"))
            {
                s.StudentFee = (from sfee in DAO.classes where sfee.classId == s.classId select sfee.classFee).First();
            }
            else
            {
                s.StudentFee = (from sfee in DAO.classes where sfee.classId == s.classId select sfee.classFeeU).First(); 
            }
                DAO.students.AddObject(s);
                DAO.SaveChanges();

                return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }
        
        //
        // GET: /Student/Edit/5
 
        public ActionResult Edit(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from fam in DAO.families where fam.familyId==id select fam).FirstOrDefault());
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, family fm)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add update logic here
                family fami = (from fam in DAO.families where fam.familyId==id select fam).FirstOrDefault();
                fami.familyId = fm.familyId;
                fami.FatherEducation = fm.FatherEducation;
                fami.FatherIncome = fm.FatherIncome;
                fami.FatherName = fm.FatherName;
                fami.FatherNIC = fm.FatherNIC;
                fami.FatherOccupation = fm.FatherOccupation;
                fami.MotherEducation = fm.MotherEducation;
                fami.MotherIncome = fm.MotherIncome;
                fami.MotherName = fm.MotherName;
                fami.MotherNIC = fm.MotherNIC;
                fami.MotherOccupation = fm.MotherOccupation;
                fami.Religion = fm.Religion;
                fami.Nationality = fm.Nationality;
                fami.Caste = fm.Caste;

                DAO.SaveChanges();

                return RedirectToAction("ExistFamily");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Student/Delete/5
 
        public ActionResult Delete(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from fam in DAO.families where fam.familyId == id select fam).FirstOrDefault());
        }

        //
        // POST: /Student/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, family fm)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add delete logic here
                family fami = (from fam in DAO.families where fam.familyId == id select fam).FirstOrDefault();
                DAO.families.DeleteObject(fami);
                DAO.SaveChanges();

                return RedirectToAction("ExistFamily");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult StudentViewMenu()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ActionResult ViewClass(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<@class> classList = new List<@class>();
            classList = DAO.classes.ToList<@class>();
            ViewBag.classList = classList;            
            ViewBag.sid = -1;

            return View();
        }        
        
        [HttpPost]
        public ActionResult ViewClass(int id, student std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                cid = Convert.ToInt32(std.classId);
                return RedirectToAction("ViewClassResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("ViewClass");
            }
            
        }

        public ActionResult ViewClassResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try{
                return View((from list in DAO.students where ((list.classId == cid) && (list.StudentStatus != 0)) select list).ToList<student>());
            }
            catch(Exception E)
            {
                return View("ViewClass");
            }
 
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult ViewSection(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> sectionList = new List<String>();
            sectionList.Add("BLUE");
            sectionList.Add("GREEN");
            sectionList.Add("RED");
            sectionList.Add("PINK");
            sectionList.Add("BROWN");
            sectionList.Add("BLACK");
            sectionList.Add("WHITE");
            sectionList.Add("ORANGE");
            sectionList.Add("PURPLE");
                     
            ViewBag.sectionList = sectionList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult ViewSection(int id, student std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                csection = std.StudyClass.ToString();
                return RedirectToAction("ViewSectionResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("ViewSection");
            }
        }

        public ActionResult ViewSectionResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.students where ((list.StudyClass == csection) && (list.StudentStatus != 0)) select list).ToList<student>());
            }
            catch (Exception E)
            {
                return View("ViewSection");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult ViewMedium(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<String> mediumList = new List<String>();
            mediumList.Add("English Medium");
            mediumList.Add("Urdu Medium");

            ViewBag.mediumList = mediumList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult ViewMedium(int id, student std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                cmedium = std.EducationMedium.ToString();
                return RedirectToAction("ViewMediumResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("ViewMedium");
            }
        }

        public ActionResult ViewMediumResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.students where ((list.EducationMedium == cmedium) && (list.StudentStatus != 0)) select list).ToList<student>());
            }
            catch (Exception E)
            {
                return View("ViewMedium");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult ViewGender(int id)
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
        public ActionResult ViewGender(int id, student std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                cgender = std.StudentGender.ToString();
                return RedirectToAction("ViewGenderResult");
            }
            catch(Exception E)
            {
                return RedirectToAction("ViewGender");
            }
        }

        public ActionResult ViewGenderResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.students where ((list.StudentGender == cgender) && (list.StudentStatus != 0)) select list).ToList<student>());
            }
            catch (Exception E)
            {
                return View("ViewGender");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult ViewAll(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<int> statusList = new List<int>();
            statusList.Add(1);
            statusList.Add(0);

            ViewBag.statusList = statusList;
            ViewBag.sid = -1;

            return View();
        }

        [HttpPost]
        public ActionResult ViewAll(int id, student std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                cstatus = Convert.ToInt32(std.StudentStatus);
                return RedirectToAction("ViewAllResult");
            }
            catch (Exception E)
            {
                return RedirectToAction("ViewAll");
            }
        }

        public ActionResult ViewAllResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                return View((from list in DAO.students where list.StudentStatus==cstatus select list).ToList<student>());
            }
            catch (Exception E)
            {
                return View("ViewAll");
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public ActionResult DeleteStudent(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            student s = (from st in DAO.students where st.studentId == id select st).FirstOrDefault<student>();
            s.StudentStatus = 0;
            DAO.SaveChanges();
            return RedirectToAction("StudentViewMenu");
        }

        public ActionResult EditStudent(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<@class> classList = new List<@class>();
            classList = DAO.classes.ToList<@class>();

            List<String> sectionList = new List<String>();
            sectionList.Add("BLUE");
            sectionList.Add("GREEN");
            sectionList.Add("RED");
            sectionList.Add("PINK");
            sectionList.Add("BROWN");
            sectionList.Add("BLACK");
            sectionList.Add("WHITE");
            sectionList.Add("ORANGE");
            sectionList.Add("PURPLE");

            List<String> genderList = new List<String>();
            genderList.Add("Male");
            genderList.Add("Female");

            List<String> nationalityList = new List<String>();
            nationalityList.Add("Pakistani");
            nationalityList.Add("Non-Pakistani");

            List<String> mediumList = new List<String>();
            mediumList.Add("English Medium");
            mediumList.Add("Urdu Medium");

            List<int> statusList = new List<int>();
            statusList.Add(1);
            statusList.Add(0);

            ViewBag.classList = classList;
            ViewBag.sectionList = sectionList;
            ViewBag.famId = id;
            ViewBag.genderList = genderList;
            ViewBag.nationalityList = nationalityList;
            ViewBag.mediumList = mediumList;
            ViewBag.statusList = statusList;
            return View((from s in DAO.students where s.studentId==id select s).FirstOrDefault<student>());
        }

        [HttpPost]
        public ActionResult EditStudent(int id, student std)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            student s = (from st in DAO.students where st.studentId == id select st).FirstOrDefault<student>();
            s.StudentStatus = std.StudentStatus;
            s.studentId = std.studentId;
            s.StudentName = std.StudentName;
            s.StudentRollNumber = std.StudentRollNumber;
            s.StudentFee = std.StudentFee;
            s.StudyClass = std.StudyClass;
            s.StudentAddress = std.StudentAddress;
            s.CellPhone = std.CellPhone;
            s.Telephone = std.Telephone;
            s.StudentGender = std.StudentGender;
            s.DateOfAdmission = std.DateOfAdmission;
            s.StudentDOB = std.StudentDOB;
            s.classId = std.classId;
            s.EducationMedium = std.EducationMedium;
            s.familyId = std.familyId;

            if (std.EducationMedium.Equals("English Medium"))
            {
                s.StudentFee = (from sfee in DAO.classes where sfee.classId == std.classId select sfee.classFee).First();
            }
            else
            {
                s.StudentFee = (from sfee in DAO.classes where sfee.classId == std.classId select sfee.classFeeU).First();
            }

            
            DAO.SaveChanges();
            return RedirectToAction("StudentViewMenu");
        }

    }

}
