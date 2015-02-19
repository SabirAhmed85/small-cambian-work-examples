using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    [Table("tblCambianSystemUsers")]
    public class SystemUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        public int RoleID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(20)]
        public string Firstname { get; set; }

        [StringLength(20)]
        public string Lastname { get; set; }

        [StringLength(100)]
        public string WindowsLogin { get; set; }

        [StringLength(5)]
        public string SignOffPin { get; set; }

        public string Fullname
        {
            get
            {
                return string.Format("{0} {1}", Firstname, Lastname);
            }
        }

        public string NameAndRole
        {
            get
            {
                return string.Format("{0} {1} ({2})", Firstname, Lastname, Role.RoleName);
            }
        }

        public virtual List<SystemUserAccessSite> AllowedSites { get; set; }
        public virtual Role Role { get; set; }

        /// <summary>
        /// Collection of sites for which this individual acts as SiteManager.
        /// </summary>
        public virtual List<Site> SitesManaged { get; set; } // see also CambianKpiContext.cs for additional configuration

        /// <summary>
        /// Collection of regions for which this individual acts as Region Manager or Ops Director
        /// </summary>
        public virtual List<KpiRegion> RegionsManaged { get; set; } // see also CambianKpiContext.cs for additional configuration
    }
}