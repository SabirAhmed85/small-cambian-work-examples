using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiScore
    {
        [Key, Column(Order = 0)]
        public int KpiMeasureID { get; set; }
        [Key, Column(Order = 1)]
        public int SiteID { get; set; }
        [Key, Column(Order = 2)]
        public int Year { get; set; }
        [Key, Column(Order = 3)]
        public int Month { get; set; }
        [Key, Column(Order = 4)]
        public int Day { get; set; }
        public float Value { get; set; }
        public int CreatedByID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedByID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Comment { get; set; }
    }
}