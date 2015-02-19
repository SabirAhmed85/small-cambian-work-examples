using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiRegion : IKpiRegion
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiRegion(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiRegion Create()
        {
            KpiRegion newOne = db.KpiRegion.Create();
            db.SaveChanges();
            return newOne;
        }
        public KpiRegion Read(int RegionID)
        {
            return db.KpiRegion.Find(RegionID);
        }
        public void Update(KpiRegion k)
        {
            KpiRegion dbOne = db.KpiRegion.Find(k.KpiRegionID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiRegion k)
        {
            db.KpiRegion.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        #endregion
    }
}