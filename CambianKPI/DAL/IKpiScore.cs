using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiScore
    {
        #region CRUD
        KpiScore Create();
        void Create(KpiScore k);
        KpiScore Read(int KpiMeasureID, int SiteID, int Year, int Month, int Day);
        void Update(KpiScore k);
        void Delete(KpiScore k);
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<KpiScore> GetAllScoresForSite(int? id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KpiMeasureID"></param>
        /// <param name="SiteID"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        int GetNumScoresPerMeasureYearSite(int KpiMeasureID, int SiteID, int Year);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        void SaveDynamicScore(KpiScore score, int UserID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="dateString"></param>
        /// <returns></returns>
        List<SiteScoresMeasuresVM> GetKpiSiteScoresMeasures(int SiteID, string dateString);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="comment"></param>
        void SaveComment(KpiScore score, string comment);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        List<KpiScore> GetScoresPerDateSiteMeasure(int SiteID, DateTime date);
        #endregion
    }
}
