using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiSiteSignoff
    {
        [Key, Column(Order = 0)]
        public int KpiCollectionID { get; set; }
        [Key, Column(Order = 1)]
        public int SiteID { get; set; }
        [Key, Column(Order = 2)]
        public int Year { get; set; }
        [Key, Column(Order = 3)]
        public int Month { get; set; }
        [Key, Column(Order = 4)]
        public int Day { get; set; }
        [Key, Column(Order = 5)]
        public int SignoffLevel { get; set; }

        [ForeignKey("SubmittedByUser")]
        public int SubmittedByUserID { get; set; }
        public virtual SystemUser SubmittedByUser { get; set; }

        public DateTime SubmittedDateTime { get; set; }
        public int SignoffStatus { get; set; }

        public string Comment { get; set; }
    }
}