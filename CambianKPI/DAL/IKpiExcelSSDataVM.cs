using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;

namespace CambianKPI.DAL
{
    public interface IKpiExcelSSDataVM
    {
        #region Methods
        List<KpiExcelSSDataVM> GetSiteScoresMeasures(int id, string date);
        #endregion

        #region Methods for Unit Testing
        KpiExcelSSDataVM Read();
        #endregion
    }
}
