using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiThresholdMeasure
    {
        #region CRUD
        KpiThresholdMeasure Create();
        void Delete(KpiThresholdMeasure k);
        #endregion

        #region Methods
        List<KpiThresholdMeasure> GetThresholdMeasuresByMeasureID(int KpiMeasureID);
        #endregion
    }
}
