using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiMeasure : IKpiMeasure
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiMeasure(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiMeasure Create()
        {
            KpiMeasure newOne = db.KpiMeasures.Create();
            db.SaveChanges();
            return newOne;
        }
        public KpiMeasure Read(int MeasureID)
        {
            return db.KpiMeasures.Find(MeasureID);
        }
        public void Update(KpiMeasure k)
        {
            KpiMeasure dbOne = db.KpiMeasures.Find(k.KpiMeasureID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiMeasure k)
        {
            db.KpiMeasures.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasuredID"></param>
        /// <returns></returns>
        public List<AffectedMeasure> GetAffectedMeasures(int MeasuredID)
        {
            mKpiCalculatedScores _kpiCalculatedScore = new mKpiCalculatedScores(db);
            
            // -- var affectedMeasures = db.KpiMeasures.Include(af => af.KpiFunction).Where(af => af.Input1MeasureID == score.KpiMeasureID || af.Input2MeasureID == score.KpiMeasureID || af.Input3MeasureID == score.KpiMeasureID).ToList();
            //new version of above
            List<KpiCalculatedScores> affectedInputsForMeasures = _kpiCalculatedScore.GetAffectedInputsByMeasure(MeasuredID);

            List<AffectedMeasure> affectedMeasures = (from af in affectedInputsForMeasures
                                                      join m in db.KpiMeasures on af.KpiMeasureID equals m.KpiMeasureID
                                                      join f in db.KpiFunctions on m.KpiFunctionID equals f.KpiFunctionID
                                                      select new AffectedMeasure() { KpiMeasureID = m.KpiMeasureID, KpiFunction = m.KpiFunction, KpiFunctionID = m.KpiFunctionID, InputNumber = af.InputNumber, InputKpiMeasureID = af.InputKpiMeasureID }).ToList();// { m.KpiMeasureID, m.KpiFunction, m.KpiFunctionID, af.InputNumber, af.InputKpiMeasureID };

            return affectedMeasures;
        }

        public KpiMeasure GetExtendedMeasureWithFunction(int MeasureID)
        {
            return db.KpiMeasures.Where(k => k.KpiMeasureID == MeasureID).Include("KpiFunction").FirstOrDefault();
        }
        #endregion
    }
}