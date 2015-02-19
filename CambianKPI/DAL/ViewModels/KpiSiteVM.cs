using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.ViewModels
{
    public partial class KpiSiteVM
    {
        public List<SiteVM> SitesVM { get; set; }

        public List<SiteVM> RowsCompleted { get; set; }
        public List<SiteVM> RowsWhichFailedTarget { get; set; }
        public System.Collections.ArrayList ArrayForRowsWhichFailedTarget { get; set; }
        public string WeekEndingUrlParam { get; set; }
    }

    public class SiteVM
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public int SubRegionID { get; set; }
        public string SubRegionName { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public string SignedOffBy { get; set; }
        public float? Capacity { get; set; }
        public float? Occupancy { get; set; }
        public float? OccPercent { get; set; }
        public float? OccChange { get; set; }
        public float? ShiftVariance { get; set; }
        public float? AgencyVariance { get; set; }
        public float? ManagerVariance { get; set; }
        public float? TeamLeaderVariance { get; set; }
        public float? RCWVariance { get; set; }
        public float? TotalOffShift { get; set; }
        public float? TotalOnShift { get; set; }
        public float? OffShiftPercent { get; set; }
        public bool UserCanEdit { get; set; }
    }
}