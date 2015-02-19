using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;

namespace CambianKPI.DAL
{
    public interface ISiteScoresMeasuresVM
    {
        #region Methods
        List<SiteScoresMeasuresVM> GetSiteScoresMeasures(int? id, string date);
        #endregion

        //#region Methods for Unit Testing
        //SiteScoresMeasuresVM Read();
        //#endregion
    }
}
