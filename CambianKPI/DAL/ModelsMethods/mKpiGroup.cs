using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiGroup : IKpiGroup
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiGroup(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiGroup Create()
        {
            KpiGroup newOne = db.KpiGroups.Create();
            db.SaveChanges();
            return newOne;
        }
        public KpiGroup Read(int GroupID)
        {
            return db.KpiGroups.Find(GroupID);
        }
        public void Update(KpiGroup k)
        {
            KpiGroup dbOne = db.KpiGroups.Find(k.KpiGroupID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiGroup k)
        {
            db.KpiGroups.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        #endregion
    }
}