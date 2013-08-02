using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using School_Management_System.Models;

namespace School_Management_System.Models
{
    public class MonthYearClassName
    {
        public int id { get; set; }
        public string name { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string section { get; set; }

        public static int giveNumberAgainstMonth(string month)
        {
            int num = 0;
            switch (month)
            {
                case "January":
                    num = 1;
                    break;
                case "Februray":
                    num = 2;
                    break;
                case "March":
                    num = 3;
                    break;
                case "April":
                    num = 4;
                    break;
                case "May":
                    num = 5;
                    break;
                case "June":
                    num = 6;
                    break;
                case "July":
                    num = 7;
                    break;
                case "August":
                    num = 8;
                    break;
                case "September":
                    num = 9;
                    break;
                case "October":
                    num = 10;
                    break;
                case "November":
                    num = 11;
                    break;
                case "December":
                    num = 12;
                    break;
            }
            return num;
        }

        public static List<SelectListItem> getSection(string className)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            schooldbEntities db = new schooldbEntities();
            int classId = (from tmp in db.classes where tmp.className == className select tmp.classId).FirstOrDefault();
            List<section> sectionList = (from tmp in db.sections where tmp.classId == classId select tmp).ToList();
            foreach (section section in sectionList)
            {
                list.Add(new SelectListItem { Selected = false, Text = section.sectionName, Value = section.sectionName});
            }
            return list;
        }
    }
}