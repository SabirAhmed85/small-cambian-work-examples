using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiFunction
    {
        public int KpiFunctionID { get; set; }

        [StringLength(50)]
        public string KpiFunctionName { get; set; }

        public int InputCount { get; set; }
        public int InputValuesCount { get; set; }
        public bool IsAccumulation { get; set; }
    }
}