using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class KpiMeasure
    {
     
        public int KpiMeasureID { get; set; }

        [StringLength(100)]
        public string KpiMeasureName { get; set; }

        public int KpiGroupID { get; set; }
        public string Guidance { get; set; }

        [ForeignKey("KpiFunction")]
        public int KpiFunctionID { get; set; }
        public KpiFunction KpiFunction { get; set; }

        public bool CentralEntry { get; set; }

        public int? Input1MeasureID { get; set; }
        public int? Input2MeasureID { get; set; }
        public int? Input3MeasureID { get; set; }
        public int? Input4MeasureID { get; set; }

        /// <summary>
        /// The benchmark value that scores should reach to measure success/failure against KPIs
        /// </summary>
        public float? TargetValue { get; set; }

        /// <summary>
        /// TRUE if the score must be equal or below target to be successful; FALSE for a minimum target, i.e. successful
        /// values must be equal or higher.
        /// </summary>
        public bool TargetIsMaximum { get; set; }

        /// <summary>
        /// TRUE if the default action is to automatically carry over the same score from one week to the next.
        /// </summary>
        public bool CarryOver { get; set; }

        public float? minThreshold { get; set; }
        public float? maxThreshold { get; set; }
        public int? relatedThresholdInputMeasureID { get; set; }
        public int? validationCount { get; set; }
        public bool? isValidationOnly { get; set; }
    }
}