using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ViewModels
{
    public partial class KpiSiteScoresMeasuresVM
    {
        public Site Site { get; set; }
        public List<SiteScoresMeasuresVM> SiteScoresMeasures { get; set; }

        public System.Collections.Hashtable measureIDsHashset { get; set; }
        public Dictionary<object, string> invalidClassesDictionary { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SiteID { get; set; }

        [StringLength(50)]
        public string SiteName { get; set; }

        public int KpiRegionID { get; set; }

        public string Type { get; set; }

        public string Manager { get; set; }

        public string RM { get; set; }

        public string Director { get; set; }

        public bool allScoresFilled { get; set; }

        public bool commentsMissing { get; set; }

        public bool allScoresValid { get; set; }

        public List<KpiSiteSignoff> ApprovalObjects { get; set; }

    }
}