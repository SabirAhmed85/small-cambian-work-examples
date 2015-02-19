using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiCalculatedScores : IKpiCalculatedScores
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiCalculatedScores(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public KpiCalculatedScores Create()
        {
            KpiCalculatedScores newOne = db.KpiCalculatedScores.Create();
            db.SaveChanges();
            return newOne;
        }
        public KpiCalculatedScores Read(int MeasureID, int InputNum)
        {
            return db.KpiCalculatedScores.Find(MeasureID, InputNum);
        }
        public void Update(KpiCalculatedScores kcs)
        {
            KpiCalculatedScores dbOne = db.KpiCalculatedScores.Find(kcs.KpiMeasureID, kcs.InputNumber);
            dbOne = kcs;
            db.SaveChanges();
        }
        public void Delete(KpiCalculatedScores kcs)
        {
            db.KpiCalculatedScores.Remove(kcs);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <returns></returns>
        public List<KpiCalculatedScores> GetAffectedInputsByMeasure(int MeasureID)
        {
            return db.KpiCalculatedScores.Where(af => af.InputKpiMeasureID == MeasureID).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <returns></returns>
        public List<KpiCalculatedScores> GetScoresByMeasureID(int MeasureID)
        {
            return db.KpiCalculatedScores.Where(s => s.KpiMeasureID == MeasureID).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <param name="InputNumber"></param>
        /// <returns></returns>
        public KpiCalculatedScores GetByMeasureInputNum(int MeasureID, int InputNumber)
        {
            return db.KpiCalculatedScores.Where(imd1 => imd1.KpiMeasureID == MeasureID && imd1.InputNumber == InputNumber).ToList().First();
        }
        #endregion
    }
}