using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiSiteSignoff : IKpiSiteSignoff
    {
        protected CambianKPI.DAL.CambianKpiContext db;

        public mKpiSiteSignoff(CambianKPI.DAL.CambianKpiContext context)
        {
            db = context;
        }
        
        #region CRUD
        public KpiSiteSignoff Create()
        {
            KpiSiteSignoff newOne = db.KpiSiteSignoff.Create();
            db.SaveChanges();
            return newOne;
        }
        public void Create(KpiSiteSignoff k)
        {
            db.KpiSiteSignoff.Add(k);
            db.SaveChanges();
        }
        public KpiSiteSignoff Read(int kCollectionID, int kSiteID, int kYear, int kMonth, int kDay, int kSignoffLevel)
        {
            return db.KpiSiteSignoff.Find(kCollectionID, kSiteID, kYear, kMonth, kDay, kSignoffLevel);
        }
        public void Update(KpiSiteSignoff k)
        {
            KpiSiteSignoff dbOne = db.KpiSiteSignoff.Find(k.KpiCollectionID, k.SiteID, k.Year, k.Month, k.Day, k.SignoffLevel);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiSiteSignoff k)
        {
            db.KpiSiteSignoff.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        public List<KpiSiteSignoff> GetSiteSignoffRecord(int? SiteID, DateTime dateObj)
        {
            return db.KpiSiteSignoff.Where(k => k.SiteID == SiteID && k.Year == dateObj.Year && k.Month == dateObj.Month && k.Day == dateObj.Day).ToList();
        }
        #endregion
    }
}