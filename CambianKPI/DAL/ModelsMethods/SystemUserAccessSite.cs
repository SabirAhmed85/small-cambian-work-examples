using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mSystemUserAccessSite : ISystemUserAccessSite
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mSystemUserAccessSite(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public SystemUserAccessSite Create()
        {
            SystemUserAccessSite newOne = db.SystemUserAccessSites.Create();
            db.SaveChanges();
            return newOne;
        }
        public SystemUserAccessSite Read(int UserID, int SiteID)
        {
            return db.SystemUserAccessSites.Find(UserID, SiteID);
        }
        public void Update(SystemUserAccessSite k)
        {
            SystemUserAccessSite dbOne = db.SystemUserAccessSites.Find(k.UserID, k.SiteID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(SystemUserAccessSite k)
        {
            db.SystemUserAccessSites.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        #endregion
    }
}