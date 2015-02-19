using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiCalculatedScores
    {
        #region CRUD
        KpiCalculatedScores Create();
        KpiCalculatedScores Read(int KpiMeasureID, int InputNumber);
        void Update(KpiCalculatedScores kcs);
        void Delete(KpiCalculatedScores kcs);
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <returns></returns>
        List<KpiCalculatedScores> GetAffectedInputsByMeasure(int KpiMeasureID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <returns></returns>
        List<KpiCalculatedScores> GetScoresByMeasureID(int KpiMeasureID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <param name="InputNumber"></param>
        /// <returns></returns>
        KpiCalculatedScores GetByMeasureInputNum(int KpiMeasureID, int InputNumber);
        #endregion
    }
}
