using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    [Table("tblRoles")]
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string AllowActiveCareForms { get; set; }
        public string AllowCarePlans { get; set; }
        public string AllowRiskSignoff { get; set; }
        public string AllowDLSOSInput { get; set; }
        public string AllowDLSOSConfig { get; set; }
        public string AllowAdminPage { get; set; }
        public string AllowDLSkillSetAssignment { get; set; }
        public string AllowEditSchoolData { get; set; }

        /// <summary>
        /// "YES" if this role can edit existing DLSOS scores.
        /// </summary>
        public string AllowDLSOSEdit { get; set; }

        /// <summary>
        /// "YES" if this role can manage SystemUser accounts.
        /// </summary>
        public string AllowUserManagement { get; set; }

        /// <summary>
        /// "YES" if this role can assign DRA versions
        /// </summary>
        public string AllowDRAVersionAssignment { get; set; }
        
        // Disabled for KPI project to prevent build errors.
        // Will not change DB schema for this model.
        //public virtual List<Measure> CanEditMeasures { get; set; }
        //public virtual List<Measure> CanSignoffMeasures { get; set; }

        /// <summary>
        /// Number of months representing the maximum age of a DLSOS score that can be edited by this role.
        /// </summary>
        public int DLEditMonthsMax { get; set; }

        public bool AllowKpiEntry { get; set; }
        public bool AllowKpiSubRegionSignoff { get; set; }
        public bool AllowKpiRegionSignoff { get; set; }
        public bool AllowKpiCentralEntry { get; set; }
    }
}