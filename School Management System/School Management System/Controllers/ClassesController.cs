using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class ClassesController : Controller
    {
        schooldbEntities DAO = new schooldbEntities();
        //
        // GET: /Classes/

        public ActionResult Index()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View(DAO.classes.ToList<@class>());
        }

       //
        // GET: /Classes/Create

        public ActionResult Create()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        } 

        //
        // POST: /Classes/Create

        [HttpPost]
        public ActionResult Create(@class c)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add insert logic here

                DAO.classes.AddObject(c);
                DAO.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Classes/Edit/5
 
        public ActionResult Edit(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from cls in DAO.classes where cls.classId==id select cls).FirstOrDefault<@class>());
        }

        //
        // POST: /Classes/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, @class c)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add update logic here

                @class myclass = (from cls in DAO.classes where cls.classId == id select cls).FirstOrDefault<@class>();
                myclass.classId = c.classId;
                myclass.className = c.className;
                myclass.classFee = c.classFee;
                DAO.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Classes/Delete/5
 
        public ActionResult Delete(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from cls in DAO.classes where cls.classId == id select cls).FirstOrDefault<@class>());
        }

        //
        // POST: /Classes/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, @class c)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add delete logic here

                @class mycls = (from cls in DAO.classes where cls.classId == id select cls).FirstOrDefault<@class>();
                DAO.classes.DeleteObject(mycls);
                DAO.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Sections

        public ActionResult viewsection(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            ViewBag.cId = id;
            return View((from slist in DAO.sections where slist.classId==id select slist).ToList<section>());
        }

       public ActionResult viewdelete(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            section s = (from ss in DAO.sections where ss.sectionId==id select ss).FirstOrDefault<section>();
            DAO.sections.DeleteObject(s);
            DAO.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult viewadd(int id)
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
            ViewBag.cId = id;
            return View(new section() { classId = id });
        }

        [HttpPost]
        public ActionResult viewadd(int id, section obj)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            DAO.sections.AddObject(obj);
            DAO.SaveChanges();

            return RedirectToAction("index");
        }

        

    }
}
