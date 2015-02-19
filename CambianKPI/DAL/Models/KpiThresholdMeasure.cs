using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiThresholdMeasure
    {
        [Key, Column(Order = 0)]
        public int KpiMeasureID { get; set; }
        [Key, Column(Order = 1)]
        public int relatedKpiMeasureID { get; set; }
    }
}