using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School_Management_System.Controllers
{
    public class AcademicController : Controller
    {
        //
        // GET: /Academic/

        public ActionResult Index()
        {
            if (Session["id"] == null)
                return  RedirectToAction("signin", "Home");
            return View();
        }

        //
        // GET: /Academic/Details/5

        public ActionResult Details(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        //
        // GET: /Academic/Create

        public ActionResult Create()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        } 

        //
        // POST: /Academic/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (Session["id"] == null)
                return  RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Academic/Edit/5
 
        public ActionResult Edit(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        //
        // POST: /Academic/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Session["id"] == null)
                return  RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Academic/Delete/5
 
        public ActionResult Delete(int id)
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        //
        // POST: /Academic/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["id"] == null)
                return  RedirectToAction("signin", "Home");
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
