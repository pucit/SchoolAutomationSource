using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Management_System.Models
{
    public class ReportStructure
    {
        public List<string> month { get; set; }
        public List<student> stdList { get; set; }
        public List<double?> feeList { get; set; }
        public List<double?> feeList1 { get; set; }
        public List<double?> feeList2 { get; set; }
        public List<double?> feeList3 { get; set; }
        public List<double?> feeList4 { get; set; }
        public List<double?> feeList5 { get; set; }
        public List<double?> feeList6 { get; set; }
        public List<double?> feeList7 { get; set; }
        public List<double?> feeList8 { get; set; }
        public List<double?> feeList9 { get; set; }
        public List<double?> feeList10 { get; set; }
        public List<double?> feeList11 { get; set; }
        public List<double?> feeList12 { get; set; }
        public double? totalFee { get; set; }
        public double? netFee { get; set; }
        public List<fee> feesList { get; set; }
        public List<fee> feesList1 { get; set; }
        public List<fee> feesList2 { get; set; }
        public List<fee> feesList3 { get; set; }
        public List<fee> feesList4 { get; set; }
        public List<fee> feesList5 { get; set; }
        public List<fee> feesList6 { get; set; }
        public List<fee> feesList7 { get; set; }
        public List<fee> feesList8 { get; set; }
        public List<fee> feesList9 { get; set; }
        public List<fee> feesList10 { get; set; }
        public List<fee> feesList11 { get; set; }
        public List<fee> feesList12 { get; set; }

        public ReportStructure()
        {
            this.month = new List<string>();
            this.month.Add("January");
            this.month.Add("February");
            this.month.Add("March");
            this.month.Add("April");
            this.month.Add("May");
            this.month.Add("June");
            this.month.Add("July");
            this.month.Add("August");
            this.month.Add("September");
            this.month.Add("October");
            this.month.Add("November");
            this.month.Add("December");
        }
    }
}