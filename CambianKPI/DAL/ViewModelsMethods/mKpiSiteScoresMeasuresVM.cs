using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Constants;
using CambianKPI.DAL.ViewModels;


namespace CambianKPI.DAL.ViewModelsMethods
{
    public partial class mKpiSiteScoresMeasuresVM : IKpiSiteScoresMeasuresVM
    {
        protected CambianKpiContext db;

        public mKpiSiteScoresMeasuresVM(CambianKpiContext d)
        {
            db = d;
        }

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<KpiSiteScoresMeasuresVM> GetKpiSiteScoresMeasures(int? SiteID, DateTime date)
        {
            return db.Database.SqlQuery<KpiSiteScoresMeasuresVM>(StoredProcedures.GetSiteScoresByDate, SiteID, date).ToList();
        }
        #endregion
    }
}