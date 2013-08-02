using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class HomeController : Controller
    {
        schooldbEntities db = new schooldbEntities();

        public ActionResult Index()
        {
            
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            
            return View();
        }

        public ActionResult SchoolIndex(login lobject)
        {
            try
            {
                if (lobject.username != null && lobject.password != null)
                {
                    login user = (from log in db.logins where log.username.Equals(lobject.username) && log.password.Equals(lobject.password) select log).First();
                    Session["id"] = user.username;
                    Session["pwd"] = user.password;
                    return View();
                }
                else {
                    return RedirectToAction("signin");            
                }
            }
            catch (Exception E)
            {
                return RedirectToAction("signin");            
            }

            
        }

        public ActionResult Family()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home" );
            return View();
        }

        public ActionResult admission()
        {
            return View();
        }

        public ActionResult contact()
        {
            return View();
        }

        public ActionResult signin()
        {
            return View();
        }

        public ActionResult signout()
        {
            Session["id"] = null;
            return RedirectToAction("Index", "Home" );            
        }

        public ActionResult about()
        {
            return View();
        }
    }
}
