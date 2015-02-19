using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mKpiScore : IKpiScore
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mKpiScore(CambianKpiContext s)
        {
            db = s;
        }
        
        #region CRUD
        public KpiScore Create()
        {
            KpiScore newOne = db.KpiScores.Create();
            db.SaveChanges();
            return newOne;
        }
        public void Create(KpiScore k)
        {
            db.KpiScores.Add(k);
            db.SaveChanges();
        }
        public KpiScore Read(int kMeasureID, int kSiteID, int kYear, int kMonth, int kDay)
        {
            return db.KpiScores.Find(kMeasureID, kSiteID, kYear, kMonth, kDay);
        }
        public void Update(KpiScore k)
        {
            KpiScore dbOne = db.KpiScores.Find(k.KpiMeasureID, k.SiteID, k.Year, k.Month, k.Day);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(KpiScore k)
        {
            db.KpiScores.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<KpiScore> GetAllScoresForSite(int? id)
        {
            return db.KpiScores.Where(s => s.SiteID == id).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeasureID"></param>
        /// <param name="SiteID"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public int GetNumScoresPerMeasureYearSite(int MeasureID, int SiteID, int Year)
        {
            return db.KpiScores.Where(ri => ri.KpiMeasureID == MeasureID && ri.SiteID == SiteID && ri.Year == Year).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="UserID"></param>
        public void SaveDynamicScore(KpiScore score, int UserID)
        {
            KpiScore oldscore = db.KpiScores.Find(score.KpiMeasureID, score.SiteID, score.Year, score.Month, score.Day);
            if (oldscore != null) // already exists so we are updating with a new dynamically produced version
            {
                oldscore.ModifiedDate = DateTime.Now;
                oldscore.ModifiedByID = UserID;
                oldscore.Value = score.Value;
                db.SaveChanges();
            }
            else
            {
                score.ModifiedDate = DateTime.Now;
                score.CreatedDate = DateTime.Now;
                score.CreatedByID = UserID;
                score.ModifiedByID = UserID;
                db.KpiScores.Add(score);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public List<SiteScoresMeasuresVM> GetKpiSiteScoresMeasures(int SiteID, string dateString)
        {
            return db.Database.SqlQuery<SiteScoresMeasuresVM>("usp_GetKpiSiteScoresMeasures @SiteID = {0}, @WeekEndingDate = {1}", SiteID, dateString).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="comment"></param>
        public void SaveComment(KpiScore score, string comment)
        {
            score.Comment = comment;
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="MeasureID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<KpiScore> GetScoresPerDateSiteMeasure(int SiteID, DateTime lastSunday)
        {
            return (from m in db.KpiMeasures
                    from s in db.KpiScores.Where(s => s.SiteID == SiteID && s.KpiMeasureID == m.KpiMeasureID 
                        && s.Year == lastSunday.Year && s.Month == lastSunday.Month && s.Day == lastSunday.Day).DefaultIfEmpty()
                    select s).ToList();

        }
        #endregion
    }
}