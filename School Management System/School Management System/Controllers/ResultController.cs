using School_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School.Controllers
{
    public class ResultController : Controller
    {
        schooldbEntities db = new schooldbEntities();
        //
        // GET: /Result/

        public ActionResult Index()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult ExamIndex()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from list in db.exams select list).ToList<exam>());
        }
        public ActionResult SubjectIndex()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");

            return View((from list in db.subjects select list).ToList<subject>());
        }
        public ActionResult ResultIndex()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.response = TempData["response"];
            return View();
        }
        public ActionResult ReportIndex()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        //Exam Actions

        public ActionResult ExamCreate()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult ExamCreate(exam obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                db.exams.AddObject(obj);
                db.SaveChanges();
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }
            return RedirectToAction("ExamIndex");
        }
        public ActionResult ExamEdit(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from exm in db.exams where exm.examId == id select exm).FirstOrDefault<exam>());
        }

        [HttpPost]
        public ActionResult ExamEdit(int id, exam obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            exam objj = (from exm in db.exams where exm.examId == id select exm).FirstOrDefault<exam>();
            objj.examName = obj.examName;
            db.SaveChanges();
            return RedirectToAction("ResultIndex");
        }

        //Subject Module


        public ActionResult SubjectCreate()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult SubjectCreate(subject obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                db.subjects.AddObject(obj);
                db.SaveChanges();
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }
            return RedirectToAction("SubjectIndex");
        }
        public ActionResult SubjectEdit(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from exm in db.subjects where exm.subjectId == id select exm).FirstOrDefault<subject>());
        }

        [HttpPost]
        public ActionResult SubjectEdit(int id, subject obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            subject objj = (from exm in db.subjects where exm.subjectId == id select exm).FirstOrDefault<subject>();
            objj.subjectName = obj.subjectName;
            db.SaveChanges();
            return RedirectToAction("SubjectIndex");
        }
        public ActionResult SubjectDelete(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from sub in db.subjects where sub.subjectId == id select sub).FirstOrDefault<subject>());
        }

        [HttpPost]
        public ActionResult SubjectDelete(int id, subject obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            subject sb = (from sub in db.subjects where sub.subjectId == id select sub).FirstOrDefault<subject>();
            db.subjects.DeleteObject(sb);
            db.SaveChanges();
            return RedirectToAction("SubjectIndex");
        }

        //Subject_Class Actions

        public ActionResult SubjectClass()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            List<SelectListItem> Ls = new List<SelectListItem>();
            var list = (from temp in db.classes select temp).ToList();
            foreach (var item in list)
            {
                Ls.Add(new SelectListItem() { Text = item.className, Value = item.classId.ToString() });
            }
            return View(Ls);
            //return View();
        }
        public ActionResult fillSubject(string val)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int vall = Convert.ToInt32(val);
            ViewBag.id = vall;
            return View((from list in db.class_subject where list.classId == vall select list).ToList<class_subject>());
        }
        public ActionResult subclassCreate(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            class_subject myobject = new class_subject();
            myobject.classId = id;
            return View(myobject);
        }
        [HttpPost]
        public ActionResult subclassCreate(class_subject obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                db.class_subject.AddObject(obj);
                db.SaveChanges();
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }
            return RedirectToAction("SubjectClass");
        }
        public ActionResult subclassDelete(int sid, int cid, class_subject obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            class_subject sb = (from sub in db.class_subject where sub.classId == cid && sub.subjectId == sid select sub).FirstOrDefault<class_subject>();
            db.class_subject.DeleteObject(sb);
            db.SaveChanges();
            return RedirectToAction("SubjectClass");
        }

        ///////////////////////// ADD Result ////////////////////////////

        public ActionResult Result()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult addResult(string classId, string subjectId, string examId)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int classi = Convert.ToInt32(classId);
            ViewBag.subject = subjectId;
            ViewBag.clas = classId;
            ViewBag.exm = examId;
            ViewBag.stdList = (from std in db.students where std.classId == classi select std).ToList<student>();
            return View();
        }

        [HttpPost]
        public ActionResult saveResult(FormCollection c)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            for (int i = 0; i < c.Count; i++)
            {
                result obj = new result();

                string str = c.GetKey(i);
                var res = str.Split(';');
                obj.studentId = Convert.ToInt32(res[0]);
                obj.csId = Convert.ToInt32(res[1]);
                obj.examId = Convert.ToInt32(res[2]);
                //     obj.obtainedMarks = Convert.ToInt32(c[str]);
                if (c[str] != null)
                {
                    try
                    {
                        obj.obtainedMarks = Convert.ToInt32(c[str]);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    result lbl = null;
                    try
                    {
                        lbl = (from sub in db.results where sub.studentId == obj.studentId && sub.examId == obj.examId && sub.csId == obj.csId select sub).First();
                    }
                    catch (Exception e)
                    { }
                    if (lbl != null)
                    {
                        if (obj.obtainedMarks == lbl.obtainedMarks)
                        {

                        }
                        else
                        {
                            lbl.obtainedMarks = obj.obtainedMarks;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.results.AddObject(obj);
                        db.SaveChanges();
                    }
                }
                else
                { }

            }
            TempData["response"] = "Result Updated Successfully...";
            return RedirectToAction("ResultIndex");
        }
        public ActionResult selectClass()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        public ActionResult classReport(int examId, int classId, int subjectId)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            class_subject obj = (from sub in db.class_subject where sub.classId == classId && sub.subjectId == subjectId select sub).FirstOrDefault<class_subject>();
            return View((from list in db.results where list.csId == obj.csId && list.examId == examId select list).ToList<result>());
        }
        public ActionResult ClassResult()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        public ActionResult ErrorPage()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult AddClassResult(string classId, string examId)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            int classi = Convert.ToInt32(classId);
            ViewBag.clas = classId;
            ViewBag.exm = examId;
            ViewBag.stdList = (from std in db.students where std.classId == classi select std).ToList<student>();
            return View();
        }
        public ActionResult saveClassResult(FormCollection c)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            result obj = new result();
            for (int i = 0; i < c.Count; i++)
            {
                string str = c.GetKey(i);
                var res = str.Split(';');
                obj.studentId = Convert.ToInt32(res[0]);
                obj.examId = Convert.ToInt32(res[1]);
                int subjectId = Convert.ToInt32(res[2]);
                int classId = Convert.ToInt32(res[3]);
                var obbj = (from cls in db.class_subject where cls.classId == classId && cls.subjectId == subjectId select cls).FirstOrDefault<class_subject>();
                obj.csId = obbj.csId;
                if (c[str] != null)
                {
                    try
                    {
                        obj.obtainedMarks = Convert.ToInt32(c[str]);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    result lbl = null;
                    try
                    {
                        lbl = (from sub in db.results where sub.studentId == obj.studentId && sub.examId == obj.examId && sub.csId == obj.csId select sub).First();
                    }
                    catch (Exception e)
                    { }
                    if (lbl != null)
                    {
                        if (obj.obtainedMarks == lbl.obtainedMarks)
                        {

                        }
                        else
                        {
                            lbl.obtainedMarks = obj.obtainedMarks;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.results.AddObject(obj);
                        db.SaveChanges();
                    }
                }
                else
                { }

            }

            TempData["response"] = "Result Updated Successfully...";
            return RedirectToAction("ResultIndex");

        }
        public ActionResult classWiseReport()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult classWiseReports(int examId, int classId)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            var item1 = (from sub in db.results where sub.examId == examId && sub.class_subject.classId == classId select sub).ToList<result>();
            ViewBag.examId = examId;

            return View(item1);
        }
        /*public ActionResult ExamEdit(int id)
        {
            return View((from exm in db.exams where exm.examId == id select exm).FirstOrDefault<exam>());
        }

        [HttpPost]
        public ActionResult ExamEdit(int id, exam obj)
        {
            exam objj = (from exm in db.exams where exm.examId == id select exm).FirstOrDefault<exam>();
            objj.examName = obj.examName;
            db.SaveChanges();
            return RedirectToAction("ExamIndex");
        }*/
    }
}
