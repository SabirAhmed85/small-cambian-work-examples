using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CambianKPI.DAL.ViewModels;

namespace CambianKPI.DAL.ViewModelsMethods
{
    public partial class mSiteScoresMeasuresVM : ISiteScoresMeasuresVM
    {
        protected CambianKpiContext db;

        public mSiteScoresMeasuresVM(CambianKpiContext d)
        {
            db = d;
        }

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<SiteScoresMeasuresVM> GetSiteScoresMeasures(int? id, string date)
        {
            return db.Database.SqlQuery<SiteScoresMeasuresVM>(CambianKPI.DAL.Constants.StoredProcedures.GetSiteScoresByDate, id, date).ToList();
        }

        //#region Methods for Unit Testing
        //public SiteScoresMeasuresVM Read()
        //{
        //    return new SiteScoresMeasuresVM();
        //}
        //#endregion
        #endregion
    }

}