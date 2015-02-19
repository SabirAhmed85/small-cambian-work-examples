using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;


namespace CambianKPI.DAL.Models
{
    [Table("tblSites")]
    public class Site
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SiteID { get; set; }

        [StringLength(50)]
        public string SiteName { get; set; }

        public int KpiRegionID { get; set; }
        public KpiRegion KpiRegion { get; set; }

        public string Type { get; set; }

        [ForeignKey("SiteManager")]
        public int SiteManagerID { get; set; }
        public SystemUser SiteManager { get; set; }
    }
}