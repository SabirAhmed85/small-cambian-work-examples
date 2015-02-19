using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiThresholdMeasure : IKpiThresholdMeasure
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiThresholdMeasure(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiThresholdMeasure Create()
        {
            KpiThresholdMeasure newOne = db.KpiThresholdMeasures.Create();
            db.SaveChanges();
            return newOne;
        }
        public void Delete(KpiThresholdMeasure k)
        {
            db.KpiThresholdMeasures.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        public List<KpiThresholdMeasure> GetThresholdMeasuresByMeasureID(int MeasureID)
        {
            return db.KpiThresholdMeasures.Where(th => th.KpiMeasureID == MeasureID).ToList();
        }
        #endregion
    }
}