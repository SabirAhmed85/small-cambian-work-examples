using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiGroup
    {
        public int KpiGroupID { get; set; }

        [StringLength(50)]
        public string KpiGroupName { get; set; }
    }
}