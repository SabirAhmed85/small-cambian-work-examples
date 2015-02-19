using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiFunction : IKpiFunction
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiFunction(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiFunction Create()
        {
            KpiFunction newOne = db.KpiFunctions.Create();
            db.SaveChanges();
            return newOne;
        }
        public KpiFunction Read(int FunctionID)
        {
            return db.KpiFunctions.Find(FunctionID);
        }
        public void Update(KpiFunction k)
        {
            KpiFunction dbOne = db.KpiFunctions.Find(k.KpiFunctionID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiFunction k)
        {
            db.KpiFunctions.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        #endregion
    }
}