using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CambianKPI.DAL;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mSite : ISite
    {
        protected CambianKpiContext db;


        #region Constructor
        public mSite(CambianKpiContext context)
        {
            db = context;
        }
        #endregion


        #region CRUD
        public Site Create()
        {
            Site newOne = db.Sites.Create();
            db.SaveChanges();
            return newOne;
        }
        public Site Read(int? kSiteID)
        {
            if (kSiteID != null)
            {
                return db.Sites.Find(kSiteID);
            }
            else
            {
                return null;
            }
        }
        public void Update(Site k)
        {
            Site dbOne = db.Sites.Find(k.SiteID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(Site k)
        {
            db.Sites.Remove(k);
            db.SaveChanges();
        }
        #endregion


        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<CambianKPI.DAL.ViewModels.SiteVM> GetAllowedSites(int kUserID, string date)
        {
            string uspWithParams = string.Format(CambianKPI.DAL.Constants.StoredProcedures.GetKpiSitesForUser, kUserID, date);
            return db.Database.SqlQuery<CambianKPI.DAL.ViewModels.SiteVM>(uspWithParams).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<SelectListItem> GetScoreDatesForSelector()
        {
            string uspWithParams = string.Format(CambianKPI.DAL.Constants.StoredProcedures.GetKpiScoreDatesForSelector);
            return db.Database.SqlQuery<SelectListItem>(uspWithParams).ToList();
        }

        public Site GetThisSiteAndRegion(int? kSiteID)
        {
            return db.Sites.Include(s => s.SiteManager).Include(s => s.KpiRegion.RegionManager).Include(s => s.KpiRegion.ParentRegion.RegionManager).First(s => s.SiteID == kSiteID);
        }
        #endregion
    }
}