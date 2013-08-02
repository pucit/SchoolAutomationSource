using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    public class SalaryController : Controller
    {
        //
        // GET: /Salary/
        schooldbEntities DAO = new schooldbEntities();
        public ActionResult Index()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View();
        }

        public ActionResult Cash()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from st in DAO.teachers where st.PaymentMode.Equals("Cash") select st).ToList<teacher>());
        }

        public ActionResult Bank()
        {
            if (Session["id"] == null)
                return RedirectToAction("signin", "Home");
            return View((from st in DAO.teachers where st.PaymentMode.Equals("Bank") select st).ToList<teacher>());
        }

    }
}
