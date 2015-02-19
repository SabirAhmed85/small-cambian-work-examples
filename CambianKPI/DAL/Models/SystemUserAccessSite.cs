using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    [Table("tblCambianSystemUsersAccessSites")]
    public class SystemUserAccessSite
    {
        [Key]
        [Column("UserID", Order=0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Key]
        [Column(Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SiteID { get; set; }

        [MaxLength(2)]
        public string Access { get; set; }

        public virtual SystemUser SystemUser { get; set; }
        public virtual Site Site { get; set; }
    }
}