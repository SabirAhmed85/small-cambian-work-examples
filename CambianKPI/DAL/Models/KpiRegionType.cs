using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiRegionType
    {
        public int KpiRegionTypeID { get; set; }

        [StringLength(50)]
        public string KpiRegionTypeName { get; set; }
    }
}