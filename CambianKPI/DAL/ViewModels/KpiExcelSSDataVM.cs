using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL.ViewModels;

using LinqToExcel;

namespace CambianKPI.DAL.ViewModels
{
    public partial class KpiExcelSSDataVM
    {
        [Key, Column(Order = 0)]
        public int SiteID { get; set; }
        [Key, Column(Order = 1)]
        public int KpiMeasureID { get; set; }

        public string Week1 { get; set; }

        public string Week2 { get; set; }

        public string Week3 { get; set; }

        public string Week4 { get; set; }

        public string Week5 { get; set; }

        public string Week6 { get; set; }
    }
}