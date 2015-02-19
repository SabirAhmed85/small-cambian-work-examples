using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.ViewModels
{
    public partial class SiteScoresMeasuresVM
    {
        public int SiteID { get; set; }
        public int KpiMeasureID { get; set; }
        public string KpiGroupName { get; set; }
        public string KpiMeasureName { get; set; }
        public string Guidance { get; set; }
        public int KpiGroupID { get; set; }
        public int KpiFunctionID { get; set; }
        public bool TargetIsMaximum { get; set; }
        public bool CentralEntry { get; set; }

        public float? TargetValue { get; set; }

        // The following properties are nullable because there may not be any score saved yet
        public float? Value { get; set; }

        public int? MonthlyMeasureID { get; set; }
        public float? MonthlyValue { get; set; }

        public string Comment { get; set; }

        public int? RelatedKpiMeasureIDToHighlight { get; set; }

        public int? validationCount { get; set; }
    }
}