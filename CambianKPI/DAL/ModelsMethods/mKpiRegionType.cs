using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiRegionType : IKpiRegionType
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiRegionType(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiRegionType Create()
        {
            KpiRegionType newOne = db.KpiRegionTypes.Create();
            db.SaveChanges();
            return newOne;
        }
        public KpiRegionType Read(int RegionTypeID)
        {
            return db.KpiRegionTypes.Find(RegionTypeID);
        }
        public void Update(KpiRegionType k)
        {
            KpiRegionType dbOne = db.KpiRegionTypes.Find(k.KpiRegionTypeID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiRegionType k)
        {
            db.KpiRegionTypes.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        #endregion
    }
}