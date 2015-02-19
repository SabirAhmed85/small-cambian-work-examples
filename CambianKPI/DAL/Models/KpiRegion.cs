using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiRegion
    {
        public int KpiRegionID { get; set; }
        [ForeignKey("ParentRegion")]
        public int? ParentID { get; set; }
        public KpiRegion ParentRegion { get; set; }

        [StringLength(50)]
        public string KpiRegionName { get; set; }

        public int KpiRegionTypeID { get; set; }

        [ForeignKey("RegionManager")]
        public int RegionManagerID { get; set; }
        public SystemUser RegionManager { get; set; }

        public List<KpiRegion> ChildRegions { get; set; }
    }
}