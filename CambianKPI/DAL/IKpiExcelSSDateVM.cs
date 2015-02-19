using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;

namespace CambianKPI.DAL
{
    public interface IKpiExcelSSDateVM
    {
        #region Methods
        List<KpiExcelSSDateVM> GetSiteScoresMeasures(int id, string date);
        #endregion

        #region Methods for Unit Testing
        KpiExcelSSDateVM Read();
        #endregion
    }
}
