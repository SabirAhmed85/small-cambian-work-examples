using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiCalculatedScores
    {
        [Key, Column(Order = 0)]
        public int KpiMeasureID { get; set; }
        [Key, Column(Order = 1)]
        public int InputNumber { get; set; }

        public int InputKpiMeasureID { get; set; }
    }
}